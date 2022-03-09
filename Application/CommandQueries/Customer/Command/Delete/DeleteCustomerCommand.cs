using Application.CommandQueries.Customer.Model;
using AutoMapper;
using MediatR;
using Service.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Interfaces;
using Infrastructure.Models;

namespace Application.CommandQueries.Customer.Command.Delete
{
    public class DeleteCustomerCommand : CommandBase, IRequest<CustomerModel>, ICommand
    {
        public int Id { get; set; }
    }

    public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand, CustomerModel>
    {
        private readonly IMapper _mapper;
        private readonly ICustomerService _customerService;
        private readonly IRepository _repository;

        public DeleteCustomerCommandHandler(IMapper mapper, ICustomerService customerService, IRepository repository)
        {
            _mapper = mapper;
            _customerService = customerService;
            _repository = repository;
        }

        public async Task<CustomerModel> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            var entity = await _customerService.SoftDelete(request.Id, cancellationToken);
            if (await _repository.SaveChangesAsync(cancellationToken) > 0)
            {
                return _mapper.Map<CustomerModel>(entity.Entity);
            }

            return null;
        }
    }
}
