using System.Collections.Generic;
 
using Application.CommandQueries.Customer.Model;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Infrastructure.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Interfaces;
using Infrastructure.Utilities;
using Persistence.SqlServer.QueryProvider;

namespace Application.CommandQueries.Customer.Query.GetAll
{
    public class GetAllCustomerQuery : Pagination, IRequest<PagedList<List<CustomerModel>>>, ISearchableRequest, IQuery
    {
        public Condition[] Conditions { get; set; }

    }

    public class GetAllCustomerQueryHandler : IRequestHandler<GetAllCustomerQuery, PagedList<List<CustomerModel>>>
    {
        private readonly ICustomerService _customerService;
        private readonly IMapper _mapper;

        public GetAllCustomerQueryHandler(IMapper mapper, ICustomerService customerService)
        {
            _mapper = mapper;
            _customerService = customerService;
        }

        public async Task<PagedList<List<CustomerModel>>> Handle(GetAllCustomerQuery request, CancellationToken cancellationToken)
        {
            var entities = _customerService
                    .GetAll()
                    .ProjectTo<CustomerModel>(_mapper.ConfigurationProvider)
                    .MakeStringSearchQuery(request.Conditions)
                    .OrderByMultipleColumns(request.OrderByProperty)
                ;

            return new PagedList<List<CustomerModel>>()
            {
                Data = await entities
                    .Paginate(request.Paginator())
                    .ToListAsync(cancellationToken),
                TotalCount = request.PageIndex <= 1
                    ? await entities
                        .CountAsync(cancellationToken)
                    : 0
            };

        }
    }
}
