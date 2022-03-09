using Application.CommandQueries.Permission.Model;
using AutoMapper;
using Infrastructure.Mappings;
using MediatR;
using Service.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Interfaces;
using Infrastructure.Models;


namespace Application.CommandQueries.Permission.Command.Update
{
    public class UpdatePermissionCommand : CommandBase, IRequest<PermissionModel>, IMapFrom<Domain.Entities.Permission>, ICommand
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Title { get; set; }
        public string UniqueName { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdatePermissionCommand, Domain.Entities.Permission>()
                .IgnoreAllNonExisting();
        }
    }


    public class UpdatePermissionCommandHandler : IRequestHandler<UpdatePermissionCommand, PermissionModel>
    {
        private readonly IMapper _mapper;
        private readonly IPermissionService _permissionService;
        private readonly IRepository _repository;

        public UpdatePermissionCommandHandler(IMapper mapper, IPermissionService permissionService, IRepository repository)
        {
            _mapper = mapper;
            _permissionService = permissionService;
            _repository = repository;
        }

        public async Task<PermissionModel> Handle(UpdatePermissionCommand request, CancellationToken cancellationToken)
        {
            var updatedEntity = await _permissionService.Update(_mapper.Map<Domain.Entities.Permission>(request), cancellationToken);

            if (await _repository.SaveChangesAsync(cancellationToken) > 0)
            {
                return _mapper.Map<PermissionModel>(updatedEntity.Entity);
            }

            return null;
        }
    }
}
