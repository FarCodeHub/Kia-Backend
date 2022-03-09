using System.Threading;
using System.Threading.Tasks;
using Application.CommandQueries.User.Model;
using AutoMapper;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using MediatR;
using Service.Interfaces;

namespace Application.CommandQueries.User.Command.Delete
{
    public class DeleteUserCommand : CommandBase, IRequest<UserModel>, ICommand
    {
        public int Id { get; set; }
    }

    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, UserModel>
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IRepository _repository;

        public DeleteUserCommandHandler(IMapper mapper, IUserService userService, IRepository repository)
        {
            _mapper = mapper;
            _userService = userService;
            _repository = repository;
        }

        public async Task<UserModel> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var entity = await _userService.SoftDelete(request.Id, cancellationToken);
            if (await _repository.SaveChangesAsync(cancellationToken) > 0)
            {
                return _mapper.Map<UserModel>(entity.Entity);
            }

            return null;
        }
    }
}
