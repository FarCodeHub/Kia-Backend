using Application.CommandQueries.RolePermission.Model;
using AutoMapper;
using MediatR;
using Service.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Interfaces;
using Infrastructure.Models;

namespace Application.CommandQueries.RolePermission.Command.Delete
{
    public class DeleteRolePermissionCommand : CommandBase, IRequest<RolePermissionModel>, ICommand
    {
        public int Id { get; set; }
    }

    public class DeleteRolePermissionCommandHandler : IRequestHandler<DeleteRolePermissionCommand, RolePermissionModel>
    {
        private readonly IMapper _mapper;
        private readonly IRolePermissionService _rolePermissionService;
        private readonly IRepository _repository;

        public DeleteRolePermissionCommandHandler(IMapper mapper, IRolePermissionService rolePermissionService, IRepository repository)
        {
            _mapper = mapper;
            _rolePermissionService = rolePermissionService;
            _repository = repository;
        }

        public async Task<RolePermissionModel> Handle(DeleteRolePermissionCommand request, CancellationToken cancellationToken)
        {
            var entity = await _rolePermissionService.SoftDelete(request.Id, cancellationToken);
            if (await _repository.SaveChangesAsync(cancellationToken) > 0)
            {
                return _mapper.Map<RolePermissionModel>(entity.Entity);
            }

            return null;
        }
    }
}
