using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Application.CommandQueries.Task.Model;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.SqlServer.QueryProvider;
using Service.Interfaces;

namespace Application.CommandQueries.Task.Query.GetAllToDue
{
    public class GetAllToDueQuery : Pagination, IRequest<PagedList<List<ToDueTaskModel>>>, ISearchableRequest, IQuery
    {
        public int EmployeeId { get; set; }
        public int UnitId { get; set; }
        public DateTime FromDueDateTime { get; set; }
        public DateTime ToDueDateTime { get; set; }
    }

    public class GetAllToDueQueryHandler : IRequestHandler<GetAllToDueQuery, PagedList<List<ToDueTaskModel>>>
    {
        private readonly ITaskService _sessionService;
        private readonly IMapper _mapper;

        public GetAllToDueQueryHandler(IMapper mapper, ITaskService sessionService)
        {
            _mapper = mapper;
            _sessionService = sessionService;
        }

        public async System.Threading.Tasks.Task<PagedList<List<ToDueTaskModel>>> Handle(GetAllToDueQuery request, CancellationToken cancellationToken)
        {
            var baseQuery = _sessionService
                .GetAll();

            if (request.UnitId > 0)
            {
                baseQuery = baseQuery.Where(x => x.Employee.UnitPosition.UnitId == request.UnitId);
            }
            else
            {
                baseQuery = baseQuery.Where(x => x.EmployeeId == request.EmployeeId);
            }

            var entities = baseQuery.Where(x =>
                        x.DuoAt >= request.FromDueDateTime &&
                                       x.DuoAt <= request.ToDueDateTime
                        && x.Status != 3)
                    .ProjectTo<ToDueTaskModel>(_mapper.ConfigurationProvider)
                    .OrderByMultipleColumns(request.OrderByProperty)
                ;

            return new PagedList<List<ToDueTaskModel>>()
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
