using System.Threading;
using System.Threading.Tasks;
using Application.CommandQueries.UserRole.Model;
using AutoMapper;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using MediatR;
using Service.Interfaces;

namespace Application.CommandQueries.UserRole.Command.Delete
{
    public class DeleteUserRoleCommand : CommandBase, IRequest<UserRoleModel>, ICommand
    {
        public int Id { get; set; }
    }

    public class DeleteUserRoleCommandHandler : IRequestHandler<DeleteUserRoleCommand, UserRoleModel>
    {
        private readonly IMapper _mapper;
        private readonly IUserRoleService _userRoleService;
        private readonly IRepository _repository;

        public DeleteUserRoleCommandHandler(IMapper mapper, IUserRoleService userRoleService, IRepository repository)
        {
            _mapper = mapper;
            _userRoleService = userRoleService;
            _repository = repository;
        }

        public async Task<UserRoleModel> Handle(DeleteUserRoleCommand request, CancellationToken cancellationToken)
        {
            var entity = await _userRoleService.SoftDelete(request.Id, cancellationToken);
            if (await _repository.SaveChangesAsync(cancellationToken) > 0)
            {
                return _mapper.Map<UserRoleModel>(entity.Entity);
            }
            return null;
        }
    }
}
