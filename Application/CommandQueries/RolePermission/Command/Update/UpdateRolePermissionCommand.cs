using Application.CommandQueries.RolePermission.Model;
using AutoMapper;
using Infrastructure.Mappings;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Interfaces;
using Infrastructure.Models;


namespace Application.CommandQueries.RolePermission.Command.Update
{
    public class UpdateRolePermissionCommand : CommandBase, IRequest<RolePermissionModel>, IMapFrom<Domain.Entities.RolePermission>, ICommand
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public int PermissionId { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateRolePermissionCommand, Domain.Entities.RolePermission>()
                .IgnoreAllNonExisting();
        }
    }


    public class UpdateRolePermissionCommandHandler : IRequestHandler<UpdateRolePermissionCommand, RolePermissionModel>
    {
        private readonly IMapper _mapper;
        private readonly IRolePermissionService _rolePermissionService;
        private readonly IRepository _repository;

        public UpdateRolePermissionCommandHandler(IMapper mapper, IRolePermissionService rolePermissionService, IRepository repository)
        {
            _mapper = mapper;
            _rolePermissionService = rolePermissionService;
            _repository = repository;
        }

        public async Task<RolePermissionModel> Handle(UpdateRolePermissionCommand request, CancellationToken cancellationToken)
        {
            var entity = await _rolePermissionService.FindById(request.Id).FirstOrDefaultAsync(cancellationToken);

            // update properties

            var updatedEntity = await _rolePermissionService.Update(entity, cancellationToken);

            if (await _repository.SaveChangesAsync(cancellationToken) > 0)
            {
                return _mapper.Map<RolePermissionModel>(updatedEntity.Entity);
            }

            return null;
        }
    }
}
