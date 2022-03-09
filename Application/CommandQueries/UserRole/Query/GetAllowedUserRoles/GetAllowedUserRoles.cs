using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.CommandQueries.UserRole.Model;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Infrastructure.CurrentUserAccessor;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.SqlServer.QueryProvider;

namespace Application.CommandQueries.UserRole.Query.GetAllowedUserRoles
{
    public class GetAllowedUserRolesQuery : Pagination, IRequest<PagedList<List<UserRoleModel>>>, ISearchableRequest, IQuery
    {
    }

    public class GetAllUserRoleQueryHandler : IRequestHandler<GetAllowedUserRolesQuery, PagedList<List<UserRoleModel>>>
    {
        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public GetAllUserRoleQueryHandler(IRepository repository, IMapper mapper, ICurrentUserAccessor currentUserAccessor)
        {
            _mapper = mapper;
            _currentUserAccessor = currentUserAccessor;
            _repository = repository;
        }

        public async Task<PagedList<List<UserRoleModel>>> Handle(GetAllowedUserRolesQuery request, CancellationToken cancellationToken)
        {
            var entities = _repository
                    .GetAll<Domain.Entities.UserRole>(c
                        => c.ConditionExpression(x => x.UserId == _currentUserAccessor.GetId()))
                    .Include(x => x.Role).ProjectTo<UserRoleModel>(_mapper.ConfigurationProvider)
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
