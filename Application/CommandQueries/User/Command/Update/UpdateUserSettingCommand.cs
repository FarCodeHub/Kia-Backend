using System.Collections.Generic;
using System.Linq;
using System.Threading;
using AutoMapper;
using Infrastructure.ConfigurationAccessor;
using Infrastructure.Exceptions;
using Infrastructure.Interfaces;
using Infrastructure.Mappings;
using Infrastructure.Models;
using Infrastructure.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces;

namespace Application.CommandQueries.User.Command.Update
{
    public class UpdateUserSettingCommand : CommandBase, IRequest<bool>, IMapFrom<UpdateUserSettingCommand>, ICommand
    {
        public int Id { get; set; }
        public string Username { get; set; } = default!;
        public string Password { get; set; }
        public string NewPassword { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateUserSettingCommand, Domain.Entities.User>()
                .IgnoreAllNonExisting();
        }
    }


    public class UpdateUserSettingCommandHandler : IRequestHandler<UpdateUserSettingCommand, bool>
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IRepository _repository;
        private readonly IUserRoleService _userRoleService;
        private readonly IConfigurationAccessor _configurationAccessor;
        private readonly IHandledErrorManager _handledErrorManager;
        public UpdateUserSettingCommandHandler(IMapper mapper, IUserService userService, IRepository repository, IUserRoleService userRoleService, IConfigurationAccessor configurationAccessor, IHandledErrorManager handledErrorManager)
        {
            _mapper = mapper;
            _userService = userService;
            _repository = repository;
            _userRoleService = userRoleService;
            _configurationAccessor = configurationAccessor;
            _handledErrorManager = handledErrorManager;
        }

        public async System.Threading.Tasks.Task<bool> Handle(UpdateUserSettingCommand request, CancellationToken cancellationToken)
        {
            var entity = await _userService.FindById(request.Id).Include(x => x.UserRoleUsers).FirstOrDefaultAsync(cancellationToken);

            if (!string.IsNullOrEmpty(request.Username))
            {
                request.Username = request.Username.ToLower();
                entity.Username = request.Username;
            }

            if (!string.IsNullOrEmpty(request.NewPassword))
            {
                if (request.NewPassword != request.Password)
                {
                    _handledErrorManager.Throw<PasswordIsWrong>();
                    entity.Password = Encryption.Symmetric.CreateMd5Hash(request.NewPassword);
                }
            }

            var updatedEntity = await _userService.Update(entity, cancellationToken);


            if (await _repository.SaveChangesAsync(cancellationToken) > 0)
            {

                return true;
            }
            return false;
        }
    }
}