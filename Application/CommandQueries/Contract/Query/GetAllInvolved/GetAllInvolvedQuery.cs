using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Application.CommandQueries.Employee.Model;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.SqlServer.QueryProvider;
using Service.Interfaces;

namespace Application.CommandQueries.Contract.Query.GetAllInvolved
{
    public class GetAllInvolvedQuery : Pagination, IRequest<PagedList<List<EmployeeModel>>>, ISearchableRequest, IQuery
    {
        public int CaseId { get; set; }
    }

    public class GetAllInvolvedQueryHandler : IRequestHandler<GetAllInvolvedQuery, PagedList<List<EmployeeModel>>>
    {
        private readonly IMapper _mapper;
        private readonly ITaskService _taskService;

        public GetAllInvolvedQueryHandler(IMapper mapper, ITaskService taskService)
        {
            _mapper = mapper;
            _taskService = taskService;
 
        }

        public async System.Threading.Tasks.Task<PagedList<List<EmployeeModel>>> Handle(GetAllInvolvedQuery request, CancellationToken cancellationToken)
        {
            var entities = _taskService.GetAll()
                .Where(x => x.CaseId == request.CaseId).Select(x => x.Employee).Distinct()
                .ProjectTo<EmployeeModel>(_mapper.ConfigurationProvider)
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
