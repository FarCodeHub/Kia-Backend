using Application.CommandQueries.Role.Model;
using AutoMapper;
using MediatR;
using Service.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Interfaces;
using Infrastructure.Models;

namespace Application.CommandQueries.Role.Command.Delete
{
    public class DeleteRoleCommand : CommandBase, IRequest<RoleModel>, ICommand
    {
        public int Id { get; set; }
    }

    public class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand, RoleModel>
    {
        private readonly IMapper _mapper;
        private readonly IRoleService _roleService;
        private readonly IRepository _repository;

        public DeleteRoleCommandHandler(IMapper mapper, IRoleService roleService, IRepository repository)
        {
            _mapper = mapper;
            _roleService = roleService;
            _repository = repository;
        }

        public async Task<RoleModel> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
        {
            var entity = await _roleService.SoftDelete(request.Id, cancellationToken);
            if (await _repository.SaveChangesAsync(cancellationToken) > 0)
            {
                return _mapper.Map<RoleModel>(entity.Entity);
            }

            return null;
        }
    }
}
