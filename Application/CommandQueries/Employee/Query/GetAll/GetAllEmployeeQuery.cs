using System.Collections.Generic;
using Application.CommandQueries.Employee.Model;
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

namespace Application.CommandQueries.Employee.Query.GetAll
{
    public class GetAllEmployeeQuery : Pagination, IRequest<PagedList<List<EmployeeModel>>>, ISearchableRequest, IQuery
    {
        public Condition[] Conditions { get; set; }

    }

    public class GetAllEmployeeQueryHandler : IRequestHandler<GetAllEmployeeQuery, PagedList<List<EmployeeModel>>>
    {
        private readonly IEmployeeService _employeeService;
        private readonly IMapper _mapper;

        public GetAllEmployeeQueryHandler(IMapper mapper, IEmployeeService employeeService)
        {
            _mapper = mapper;
            _employeeService = employeeService;
        }

        public async Task<PagedList<List<EmployeeModel>>> Handle(GetAllEmployeeQuery request, CancellationToken cancellationToken)
        {
            var entities = _employeeService
                    .GetAll()
                    .ProjectTo<EmployeeModel>(_mapper.ConfigurationProvider)
                    .MakeStringSearchQuery(request.Conditions)
                    .OrderByMultipleColumns(request.OrderByProperty)
                ;

            return new PagedList<List<EmployeeModel>>()
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
