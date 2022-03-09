using Application.CommandQueries.Role.Model;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Interfaces;


namespace Application.CommandQueries.Role.Query.Get
{
    public class GetRoleQuery : IRequest<RoleModel>, IQuery
    {
        public int Id { get; set; }
    }

    public class GetRoleQueryHandler : IRequestHandler<GetRoleQuery, RoleModel>
    {
        private readonly IRoleService _roleService;
        private readonly IMapper _mapper;

        public GetRoleQueryHandler(IMapper mapper, IRoleService roleService)
        {
            _mapper = mapper;
            _roleService = roleService;
        }

        public async Task<RoleModel> Handle(GetRoleQuery request, CancellationToken cancellationToken)
        {
            var entity = _roleService.FindById(request.Id);

            return await entity
                .ProjectTo<RoleModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        }
    }
}
