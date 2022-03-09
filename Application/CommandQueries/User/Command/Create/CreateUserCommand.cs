using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.CommandQueries.User.Model;
using AutoMapper;
using Infrastructure.ConfigurationAccessor;
using Infrastructure.Exceptions;
using Infrastructure.Interfaces;
using Infrastructure.Mappings;
using Infrastructure.Models;
using Infrastructure.Utilities;
using MediatR;
using Service.Interfaces;

namespace Application.CommandQueries.User.Command.Create
{
    public class CreateUserCommand : CommandBase, IRequest<UserModel>, IMapFrom<CreateUserCommand>, ICommand
    {
        public int PersonId { get; set; } = default!;
        public string Username { get; set; } = default!;
        public string Password { get; set; }
        public IList<int> RolesIdList { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateUserCommand, Domain.Entities.User>()
                .IgnoreAllNonExisting();
        }
    }

    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserModel>
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IRepository _repository;
        private readonly IUserRoleService _userRoleService;
        private readonly IConfigurationAccessor _configurationAccessor;
        private readonly IHandledErrorManager _handledErrorManager;
        public CreateUserCommandHandler(IMapper mapper, IUserService userService, IRepository repository, IUserRoleService userRoleService, IConfigurationAccessor configurationAccessor, IHandledErrorManager handledErrorManager)
        {
            _mapper = mapper;
            _userService = userService;
            _repository = repository;
            _userRoleService = userRoleService;
            _configurationAccessor = configurationAccessor;
            _handledErrorManager = handledErrorManager;
        }


        public async Task<UserModel> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = _mapper.Map<Domain.Entities.User>(request);

            user.Username = request.Username.ToLower();
            user.Password = Encryption.Symmetric.CreateMd5Hash(user.Password);

            var entity = await _userService.Add(user);

            foreach (var i in request.RolesIdList)
            {
                await _userRoleService.Add(new Domain.Entities.UserRole() { User = entity.Entity, RoleId = i });
            }


            if (await _repository.SaveChangesAsync(cancellationToken) > 0)
            {
                return _mapper.Map<UserModel>(entity.Entity);
            }
            return null;
        }
    }
}
