using System.Threading;
using System.Threading.Tasks;
using Application.CommandQueries.UserRole.Model;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Infrastructure.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces;

namespace Application.CommandQueries.UserRole.Query.Get
{
    public class GetUserRoleQuery : IRequest<UserRoleModel>, IQuery
    {
        public int Id { get; set; }
    }

    public class GetUserRoleQueryHandler : IRequestHandler<GetUserRoleQuery, UserRoleModel>
    {
        private readonly IUserRoleService _userRoleService;
        private readonly IMapper _mapper;

        public GetUserRoleQueryHandler(IMapper mapper, IUserRoleService userRoleService)
        {
            _mapper = mapper;
            _userRoleService = userRoleService;
        }

        public async Task<UserRoleModel> Handle(GetUserRoleQuery request, CancellationToken cancellationToken)
        {
            var entity = _userRoleService.FindById(request.Id);

            return await entity
                .ProjectTo<UserRoleModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        }
    }
}
