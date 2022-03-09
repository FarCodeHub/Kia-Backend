using System.Collections.Generic;
 
using System.Threading;
using System.Threading.Tasks;
using Application.CommandQueries.User.Model;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using Infrastructure.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.SqlServer.QueryProvider;
using Service.Interfaces;

namespace Application.CommandQueries.User.Query.GetAll
{
    public class GetAllUserQuery : Pagination, IRequest<PagedList<List<UserModel>>>, ISearchableRequest, IQuery
    {
        public Condition[] Conditions { get; set; }

    }

    public class GetAllUserQueryHandler : IRequestHandler<GetAllUserQuery, PagedList<List<UserModel>>>
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public GetAllUserQueryHandler(IMapper mapper, IUserService userService)
        {
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<PagedList<List<UserModel>>> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
        {
            var entities = _userService
                    .GetAll()
                    .ProjectTo<UserModel>(_mapper.ConfigurationProvider)
                    .MakeStringSearchQuery(request.Conditions)
                    .OrderByMultipleColumns(request.OrderByProperty)
                ;

            return new PagedList<List<UserModel>>()
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
