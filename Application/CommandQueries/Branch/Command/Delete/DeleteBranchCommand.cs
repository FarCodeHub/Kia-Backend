using Application.CommandQueries.Branch.Model;
using AutoMapper;
using MediatR;
using Service.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Interfaces;
using Infrastructure.Models;

namespace Application.CommandQueries.Branch.Command.Delete
{
    public class DeleteBranchCommand : CommandBase, IRequest<BranchModel>, ICommand
    {
        public int Id { get; set; }
    }

    public class DeleteBranchCommandHandler : IRequestHandler<DeleteBranchCommand, BranchModel>
    {
        private readonly IMapper _mapper;
        private readonly IBranchService _branchService;
        private readonly IRepository _repository;

        public DeleteBranchCommandHandler(IMapper mapper, IBranchService branchService, IRepository repository)
        {
            _mapper = mapper;
            _branchService = branchService;
            _repository = repository;
        }

        public async Task<BranchModel> Handle(DeleteBranchCommand request, CancellationToken cancellationToken)
        {
            var entity = await _branchService.SoftDelete(request.Id, cancellationToken);
            try
            {
                if (await _repository.SaveChangesAsync(cancellationToken) > 0)
                {
                    return _mapper.Map<BranchModel>(entity.Entity);
                }
            }
            catch
            {
                return _mapper.Map<BranchModel>(entity.Entity);
            }

            return null;
        }
    }
}
