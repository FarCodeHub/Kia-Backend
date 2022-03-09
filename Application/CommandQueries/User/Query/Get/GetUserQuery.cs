using System.Threading;
using System.Threading.Tasks;
using Application.CommandQueries.User.Model;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Infrastructure.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces;

namespace Application.CommandQueries.User.Query.Get
{
    public class GetUserQuery : IRequest<UserModel>, IQuery
    {
        public int Id { get; set; }
    }

    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserModel>
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public GetUserQueryHandler(IMapper mapper, IUserService userService)
        {
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<UserModel> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var entity = _userService.FindById(request.Id);

            return await entity
                .ProjectTo<UserModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        }
    }
}
