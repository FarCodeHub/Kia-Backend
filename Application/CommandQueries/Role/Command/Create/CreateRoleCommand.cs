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

namespace Application.CommandQueries.Role.Command.Create
{
    public class CreateRoleCommand : CommandBase, IRequest<RoleModel>, IMapFrom<CreateRoleCommand>, ICommand
    {
        public string Title { get; set; }
        public string UniqueName { get; set; }
        public string Description { get; set; }
        public int? ParentId { get; set; }
        public List<int> PermissionsIdList { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateRoleCommand, Domain.Entities.Role>()
                .IgnoreAllNonExisting();
        }
    }

    public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, RoleModel>
    {
        private readonly IMapper _mapper;
        private readonly IRoleService _roleService;
        private readonly IRepository _repository;
        public CreateRoleCommandHandler(IMapper mapper, IRoleService roleService, IRepository repository)
        {
            _mapper = mapper;
            _roleService = roleService;
            _repository = repository;
        }


        public async Task<RoleModel> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            var role = await _roleService.Add(_mapper.Map<Domain.Entities.Role>(request),request.PermissionsIdList);

          
            if (await _repository.SaveChangesAsync(cancellationToken) > 0)
            {
                return _mapper.Map<RoleModel>(role.Entity);
            }

            return null;
        }
    }
}
