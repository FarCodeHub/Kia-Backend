using Application.CommandQueries.RolePermission.Model;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Interfaces;


namespace Application.CommandQueries.RolePermission.Query.Get
{
    public class GetRolePermissionQuery : IRequest<RolePermissionModel>, IQuery
    {
        public int Id { get; set; }
    }

    public class GetRolePermissionQueryHandler : IRequestHandler<GetRolePermissionQuery, RolePermissionModel>
    {
        private readonly IRolePermissionService _rolePermissionService;
        private readonly IMapper _mapper;

        public GetRolePermissionQueryHandler(IMapper mapper, IRolePermissionService rolePermissionService)
        {
            _mapper = mapper;
            _rolePermissionService = rolePermissionService;
        }

        public async Task<RolePermissionModel> Handle(GetRolePermissionQuery request, CancellationToken cancellationToken)
        {
            var entity = _rolePermissionService.FindById(request.Id);

            return await entity
                .ProjectTo<RolePermissionModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        }
    }
}
