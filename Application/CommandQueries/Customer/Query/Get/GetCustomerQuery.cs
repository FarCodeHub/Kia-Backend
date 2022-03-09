using Application.CommandQueries.Customer.Model;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Interfaces;


namespace Application.CommandQueries.Customer.Query.Get
{
    public class GetCustomerQuery : IRequest<CustomerModel>, IQuery
    {
        public int Id { get; set; }
    }

    public class GetCustomerQueryHandler : IRequestHandler<GetCustomerQuery, CustomerModel>
    {
        private readonly ICustomerService _customerService;
        private readonly IMapper _mapper;

        public GetCustomerQueryHandler(IMapper mapper, ICustomerService customerService)
        {
            _mapper = mapper;
            _customerService = customerService;
        }

        public async Task<CustomerModel> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
        {
            var entity = _customerService.FindById(request.Id);

            return await entity
                .ProjectTo<CustomerModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
