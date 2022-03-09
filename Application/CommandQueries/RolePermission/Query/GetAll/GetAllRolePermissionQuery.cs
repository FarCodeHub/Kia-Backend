using System.Collections.Generic;
 
using Application.CommandQueries.RolePermission.Model;
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

namespace Application.CommandQueries.RolePermission.Query.GetAll
{
    public class GetAllRolePermissionQuery : Pagination, IRequest<PagedList<List<RolePermissionModel>>>, ISearchableRequest, IQuery
    {
        public List<Condition> Queries { get; set; }

    }

    public class GetAllRolePermissionQueryHandler : IRequestHandler<GetAllRolePermissionQuery, PagedList<List<RolePermissionModel>>>
    {
        private readonly IRolePermissionService _rolePermissionService;
        private readonly IMapper _mapper;

        public GetAllRolePermissionQueryHandler(IMapper mapper, IRolePermissionService rolePermissionService)
        {
            _mapper = mapper;
            _rolePermissionService = rolePermissionService;
        }

        public async Task<PagedList<List<RolePermissionModel>>> Handle(GetAllRolePermissionQuery request, CancellationToken cancellationToken)
        {
            var entities = _rolePermissionService
                    .GetAll()
                    .ProjectTo<RolePermissionModel>(_mapper.ConfigurationProvider)
                    .MakeStringSearchQuery(request.Queries)
                    .OrderByMultipleColumns(request.OrderByProperty)
                ;

            return new PagedList<List<RolePermissionModel>>()
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
