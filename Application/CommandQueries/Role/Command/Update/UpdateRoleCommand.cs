using System.Collections.Generic;
using Application.CommandQueries.Role.Model;
using AutoMapper;
using Infrastructure.Mappings;
using MediatR;
using Service.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Interfaces;
using Infrastructure.Models;


namespace Application.CommandQueries.Role.Command.Update
{
    public class UpdateRoleCommand : CommandBase, IRequest<RoleModel>, IMapFrom<Domain.Entities.Role>, ICommand
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string UniqueName { get; set; }
        public string Description { get; set; }
        public int? ParentId { get; set; }
        public List<int> PermissionsIdList { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateRoleCommand, Domain.Entities.Role>()
                .IgnoreAllNonExisting();
        }
    }


    public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, RoleModel>
    {
        private readonly IMapper _mapper;
        private readonly IRoleService _roleService;
        private readonly IRepository _repository;

        public UpdateRoleCommandHandler(IMapper mapper, IRoleService roleService, IRepository repository)
        {
            _mapper = mapper;
            _roleService = roleService;
            _repository = repository;
        }

        public async Task<RoleModel> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
        {
            var updateEntity = await _roleService.Update(_mapper.Map<Domain.Entities.Role>(request),request.PermissionsIdList,cancellationToken);

            if (await _repository.SaveChangesAsync(cancellationToken) > 0)
            {
                return _mapper.Map<RoleModel>(updateEntity.Entity);
            }

            return null;
        }
    }
}
