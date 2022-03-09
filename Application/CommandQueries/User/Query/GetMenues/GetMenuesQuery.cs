using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.CommandQueries.User.Model;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Infrastructure.CurrentUserAccessor;
using Infrastructure.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces;

namespace Application.CommandQueries.User.Query.GetMenues
{
    public class GetMenuesQuery : IRequest<List<MenueItemModel>>, IQuery
    {
    }

    public class GetMenuesQueryHandler : IRequestHandler<GetMenuesQuery, List<MenueItemModel>>
    {
        private readonly IUserService _userService;
        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly IMapper _mapper;
        public GetMenuesQueryHandler(IUserService userService, ICurrentUserAccessor currentUserAccessor, IMapper mapper)
        {
            _userService = userService;
            _currentUserAccessor = currentUserAccessor;
            _mapper = mapper;
        }

        public async Task<List<MenueItemModel>> Handle(GetMenuesQuery request, CancellationToken cancellationToken)
        {
            var entity = await _userService.GetUserMenues(_currentUserAccessor.GetId());
            return await entity
                .ProjectTo<MenueItemModel>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
    }
}