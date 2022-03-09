using System.Threading;
using System.Threading.Tasks;
using Application.CommandQueries.Customer.Model;
using AutoMapper;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using MediatR;
using Service.Interfaces;

namespace Application.CommandQueries.Customer.Command.Merge
{
    public class CreateMergeCommand : CommandBase, IRequest<CustomerModel>, ICommand
    {
        public int BaseId { get; set; }
        public int MergeId { get; set; }
    }

    public class CreateMergeCommandHandler : IRequestHandler<CreateMergeCommand, CustomerModel>
    {
        private readonly IMapper _mapper;
        private readonly ICustomerService _customerService;
        private readonly IRepository _repository;

        public CreateMergeCommandHandler(IMapper mapper, ICustomerService customerService, IRepository repository)
        {
            _mapper = mapper;
            _customerService = customerService;
            _repository = repository;
        }

        public async Task<CustomerModel> Handle(CreateMergeCommand request, CancellationToken cancellationToken)
        {
            var entity = await _customerService.Merge(request.BaseId, request.MergeId);
            if (await _repository.SaveChangesAsync(cancellationToken) > 0)
            {
                return _mapper.Map<CustomerModel>(entity.Entity);
            }

            return null;
        }
    }
}