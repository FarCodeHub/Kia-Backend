using Application.CommandQueries.Permission.Model;
using AutoMapper;
using MediatR;
using Service.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Interfaces;
using Infrastructure.Models;

namespace Application.CommandQueries.Permission.Command.Delete
{
    public class DeletePermissionCommand : CommandBase, IRequest<PermissionModel>, ICommand
    {
        public int Id { get; set; }
    }

    public class DeletePermissionCommandHandler : IRequestHandler<DeletePermissionCommand, PermissionModel>
    {
        private readonly IMapper _mapper;
        private readonly IPermissionService _permissionService;
        private readonly IRepository _repository;

        public DeletePermissionCommandHandler(IMapper mapper, IPermissionService permissionService, IRepository repository)
        {
            _mapper = mapper;
            _permissionService = permissionService;
            _repository = repository;
        }

        public async Task<PermissionModel> Handle(DeletePermissionCommand request, CancellationToken cancellationToken)
        {
            var entity = await _permissionService.SoftDelete(request.Id, cancellationToken);
            if (await _repository.SaveChangesAsync(cancellationToken) > 0)
            {
                return _mapper.Map<PermissionModel>(entity.Entity);
            }

            return null;
        }
    }
}
