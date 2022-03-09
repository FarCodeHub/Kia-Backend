using System.Collections.Generic;
using System.Linq;
using Application.CommandQueries.Project.Model;
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

namespace Application.CommandQueries.Project.Query.GetAll
{
    public class GetAllProjectQuery : Pagination, IRequest<PagedList<List<ProjectModel>>>, ISearchableRequest, IQuery
    {
        public Condition[] Conditions { get; set; }

    }

    public class GetAllProjectQueryHandler : IRequestHandler<GetAllProjectQuery, PagedList<List<ProjectModel>>>
    {
        private readonly IProjectService _projectService;
        private readonly IMapper _mapper;

        public GetAllProjectQueryHandler(IMapper mapper, IProjectService projectService)
        {
            _mapper = mapper;
            _projectService = projectService;
        }

        public async Task<PagedList<List<ProjectModel>>> Handle(GetAllProjectQuery request, CancellationToken cancellationToken)
        {
            var entities = _projectService
                    .GetAll().Where(x=>x.Parent == null)
                    .ProjectTo<ProjectModel>(_mapper.ConfigurationProvider)
                    .MakeStringSearchQuery(request.Conditions)
                    .OrderByMultipleColumns(request.OrderByProperty)
                ;

            return new PagedList<List<ProjectModel>>()
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
