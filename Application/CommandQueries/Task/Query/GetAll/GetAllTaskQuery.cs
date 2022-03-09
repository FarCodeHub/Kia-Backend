using System.Collections.Generic;
 
using System.Threading;
using System.Threading.Tasks;
using Application.CommandQueries.Task.Model;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using Infrastructure.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.SqlServer.QueryProvider;
using Service.Interfaces;

namespace Application.CommandQueries.Task.Query.GetAll
{
    public class GetAllTaskQuery : Pagination, IRequest<PagedList<List<TaskModel>>>, ISearchableRequest, IQuery
    {
        public Condition[] Conditions { get; set; }
    }

    public class GetAllTaskQueryHandler : IRequestHandler<GetAllTaskQuery, PagedList<List<TaskModel>>>
    {
        private readonly ITaskService _sessionService;
        private readonly IMapper _mapper;

        public GetAllTaskQueryHandler(IMapper mapper, ITaskService sessionService)
        {
            _mapper = mapper;
            _sessionService = sessionService;
        }

        public async Task<PagedList<List<TaskModel>>> Handle(GetAllTaskQuery request, CancellationToken cancellationToken)
        {
            var entities = _sessionService
                    .GetAll()
                    .ProjectTo<TaskModel>(_mapper.ConfigurationProvider)
                    .MakeStringSearchQuery(request.Conditions)
                    .OrderByMultipleColumns(request.OrderByProperty)
                ;

            return new PagedList<List<TaskModel>>()
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
