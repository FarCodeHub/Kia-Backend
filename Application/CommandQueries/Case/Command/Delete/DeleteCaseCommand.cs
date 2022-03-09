using Application.CommandQueries.Case.Model;
using AutoMapper;
using MediatR;
using Service.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Interfaces;
using Infrastructure.Models;

namespace Application.CommandQueries.Case.Command.Delete
{
    public class DeleteCaseCommand : CommandBase, IRequest<CaseModel>, ICommand
    {
        public int Id { get; set; }
    }

    public class DeleteCaseCommandHandler : IRequestHandler<DeleteCaseCommand, CaseModel>
    {
        private readonly IMapper _mapper;
        private readonly ICaseService _contractService;
        private readonly IRepository _repository;

        public DeleteCaseCommandHandler(IMapper mapper, ICaseService contractService, IRepository repository)
        {
            _mapper = mapper;
            _contractService = contractService;
            _repository = repository;
        }

        public async Task<CaseModel> Handle(DeleteCaseCommand request, CancellationToken cancellationToken)
        {
            var entity = await _contractService.SoftDelete(request.Id, cancellationToken);
            if (await _repository.SaveChangesAsync(cancellationToken) > 0)
            {
                return _mapper.Map<CaseModel>(entity.Entity);
            }
            return null;
        }
    }
}
