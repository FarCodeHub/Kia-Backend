using Application.CommandQueries.RolePermission.Model;
using AutoMapper;
using Infrastructure.Mappings;
using MediatR;
using Service.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Interfaces;
using Infrastructure.Models;

namespace Application.CommandQueries.RolePermission.Command.Create
{
    public class CreateRolePermissionCommand : CommandBase, IRequest<RolePermissionModel>, IMapFrom<CreateRolePermissionCommand>, ICommand
    {
        public int RoleId { get; set; }
        public int PermissionId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateRolePermissionCommand, Domain.Entities.RolePermission>()
                .IgnoreAllNonExisting();
        }
    }

    public class CreateRolePermissionCommandHandler : IRequestHandler<CreateRolePermissionCommand, RolePermissionModel>
    {
        private readonly IMapper _mapper;
        private readonly IRolePermissionService _rolePermissionService;
        private readonly IRepository _repository;

        public CreateRolePermissionCommandHandler(IMapper mapper, IRolePermissionService rolePermissionService, IRepository repository)
        {
            _mapper = mapper;
            _rolePermissionService = rolePermissionService;
            _repository = repository;
        }


        public async Task<RolePermissionModel> Handle(CreateRolePermissionCommand request, CancellationToken cancellationToken)
        {
            var entity = await _rolePermissionService.Add(_mapper.Map<Domain.Entities.RolePermission>(request));
            if (await _repository.SaveChangesAsync(cancellationToken) > 0)
            {
                return _mapper.Map<RolePermissionModel>(entity.Entity);
            }

            return null;
        }
    }
}
