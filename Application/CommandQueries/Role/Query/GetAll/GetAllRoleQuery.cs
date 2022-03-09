using System.Collections.Generic;
using Application.CommandQueries.Role.Model;
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

namespace Application.CommandQueries.Role.Query.GetAll
{
    public class GetAllRoleQuery : Pagination, IRequest<PagedList<List<RoleModel>>>, ISearchableRequest, IQuery
    {
        public Condition[] Conditions { get; set; }

    }

    public class GetAllRoleQueryHandler : IRequestHandler<GetAllRoleQuery, PagedList<List<RoleModel>>>
    {
        private readonly IRoleService _roleService;
        private readonly IMapper _mapper;

        public GetAllRoleQueryHandler(IMapper mapper, IRoleService roleService)
        {
            _mapper = mapper;
            _roleService = roleService;
        }

        public async Task<PagedList<List<RoleModel>>> Handle(GetAllRoleQuery request, CancellationToken cancellationToken)
        {
            var entities = _roleService
                    .GetAll()
                    .Include(x => x.RolePermissionRoles)
                    .ThenInclude(x => x.Permission)
                    .ProjectTo<RoleModel>(_mapper.ConfigurationProvider)
                    .MakeStringSearchQuery(request.Conditions)
                    .OrderByMultipleColumns(request.OrderByProperty)
                ;

            return new PagedList<List<RoleModel>>()
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
