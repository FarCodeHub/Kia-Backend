using System.Collections.Generic;
 
using System.Threading;
using System.Threading.Tasks;
using Application.CommandQueries.UserRole.Model;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using Infrastructure.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.SqlServer.QueryProvider;
using Service.Interfaces;

namespace Application.CommandQueries.UserRole.Query.GetAll
{
    public class GetAllUserRoleQuery : Pagination, IRequest<PagedList<List<UserRoleModel>>>, ISearchableRequest, IQuery
    {
        public Condition[] Conditions { get; set; }

    }

    public class GetAllUserRoleQueryHandler : IRequestHandler<GetAllUserRoleQuery, PagedList<List<UserRoleModel>>>
    {
        private readonly IUserRoleService _userRoleService;
        private readonly IMapper _mapper;

        public GetAllUserRoleQueryHandler(IMapper mapper, IUserRoleService userRoleService)
        {
            _mapper = mapper;
            _userRoleService = userRoleService;
        }

        public async Task<PagedList<List<UserRoleModel>>> Handle(GetAllUserRoleQuery request, CancellationToken cancellationToken)
        {
            var entities = _userRoleService
                    .GetAll()
                    .ProjectTo<UserRoleModel>(_mapper.ConfigurationProvider)
                    .MakeStringSearchQuery(request.Conditions)
                    .OrderByMultipleColumns(request.OrderByProperty)
                ;

            return new PagedList<List<UserRoleModel>>()
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
