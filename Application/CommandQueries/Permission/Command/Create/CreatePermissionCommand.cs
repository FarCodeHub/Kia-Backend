using Application.CommandQueries.Permission.Model;
using AutoMapper;
using Infrastructure.Mappings;
using MediatR;
using Service.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Interfaces;
using Infrastructure.Models;

namespace Application.CommandQueries.Permission.Command.Create
{
    public class CreatePermissionCommand : CommandBase, IRequest<PermissionModel>, IMapFrom<CreatePermissionCommand>, ICommand
    {
        public int? ParentId { get; set; }
        public string Title { get; set; }
        public string UniqueName { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreatePermissionCommand, Domain.Entities.Permission>()
                .IgnoreAllNonExisting();
        }
    }

    public class CreatePermissionCommandHandler : IRequestHandler<CreatePermissionCommand, PermissionModel>
    {
        private readonly IMapper _mapper;
        private readonly IPermissionService _permissionService;
        private readonly IRepository _repository;

        public CreatePermissionCommandHandler(IMapper mapper, IPermissionService permissionService, IRepository repository)
        {
            _mapper = mapper;
            _permissionService = permissionService;
            _repository = repository;
        }


        public async Task<PermissionModel> Handle(CreatePermissionCommand request, CancellationToken cancellationToken)
        {
            var entity = await _permissionService.Add(_mapper.Map<Domain.Entities.Permission>(request));
            if (await _repository.SaveChangesAsync(cancellationToken) > 0)
            {
                return _mapper.Map<PermissionModel>(entity.Entity);
            }

            return null;
        }
    }
}
