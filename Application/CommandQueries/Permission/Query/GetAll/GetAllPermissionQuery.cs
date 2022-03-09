using System.Collections.Generic;
 
using Application.CommandQueries.Permission.Model;
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

namespace Application.CommandQueries.Permission.Query.GetAll
{
    public class GetAllPermissionQuery : Pagination, IRequest<PagedList<List<PermissionModel>>>, ISearchableRequest, IQuery
    {
        public Condition[] Conditions { get; set; }

    }

    public class GetAllPermissionQueryHandler : IRequestHandler<GetAllPermissionQuery, PagedList<List<PermissionModel>>>
    {
        private readonly IPermissionService _permissionService;
        private readonly IMapper _mapper;

        public GetAllPermissionQueryHandler(IMapper mapper, IPermissionService permissionService)
        {
            _mapper = mapper;
            _permissionService = permissionService;
        }

        public async Task<PagedList<List<PermissionModel>>> Handle(GetAllPermissionQuery request, CancellationToken cancellationToken)
        {
            var entities = _permissionService
                    .GetAll()
                    .ProjectTo<PermissionModel>(_mapper.ConfigurationProvider)
                    .MakeStringSearchQuery(request.Conditions)
                    .OrderByMultipleColumns(request.OrderByProperty)
                ;

            return new PagedList<List<PermissionModel>>()
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
