using Application.CommandQueries.Permission.Model;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Interfaces;


namespace Application.CommandQueries.Permission.Query.Get
{
    public class GetPermissionQuery : IRequest<PermissionModel>, IQuery
    {
        public int Id { get; set; }
    }

    public class GetPermissionQueryHandler : IRequestHandler<GetPermissionQuery, PermissionModel>
    {
        private readonly IPermissionService _permissionService;
        private readonly IMapper _mapper;

        public GetPermissionQueryHandler(IMapper mapper, IPermissionService permissionService)
        {
            _mapper = mapper;
            _permissionService = permissionService;
        }

        public async Task<PermissionModel> Handle(GetPermissionQuery request, CancellationToken cancellationToken)
        {
            var entity = _permissionService.FindById(request.Id);

            return await entity
                .ProjectTo<PermissionModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        }
    }
}
