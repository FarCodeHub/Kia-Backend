using System.Threading;
using System.Threading.Tasks;
using Application.CommandQueries.UserRole.Model;
using AutoMapper;
using Infrastructure.Interfaces;
using Infrastructure.Mappings;
using Infrastructure.Models;
using MediatR;
using Service.Interfaces;

namespace Application.CommandQueries.UserRole.Command.Create
{
    public class CreateUserRoleCommand : CommandBase, IRequest<UserRoleModel>, IMapFrom<CreateUserRoleCommand>, ICommand
    {
        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateUserRoleCommand, Domain.Entities.UserRole>()
                .IgnoreAllNonExisting();
        }
    }

    public class CreateUserRoleCommandHandler : IRequestHandler<CreateUserRoleCommand, UserRoleModel>
    {
        private readonly IMapper _mapper;
        private readonly IUserRoleService _userRoleService;
        private readonly IRepository _repository;

        public CreateUserRoleCommandHandler(IMapper mapper, IUserRoleService userRoleService, IRepository repository)
        {
            _mapper = mapper;
            _userRoleService = userRoleService;
            _repository = repository;
        }


        public async Task<UserRoleModel> Handle(CreateUserRoleCommand request, CancellationToken cancellationToken)
        {
            var entity = await _userRoleService.Add(_mapper.Map<Domain.Entities.UserRole>(request));
            if (await _repository.SaveChangesAsync(cancellationToken) > 0)
            {
                return _mapper.Map<UserRoleModel>(entity.Entity);
            }
            return null;
        }
    }
}
