using Application.CommandQueries.Position.Model;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Interfaces;


namespace Application.CommandQueries.Position.Query.Get
{
    public class GetPositionQuery : IRequest<PositionModel>, IQuery
    {
        public int Id { get; set; }
    }

    public class GetPositionQueryHandler : IRequestHandler<GetPositionQuery, PositionModel>
    {
        private readonly IPositionService _positionService;
        private readonly IMapper _mapper;

        public GetPositionQueryHandler(IMapper mapper, IPositionService positionService)
        {
            _mapper = mapper;
            _positionService = positionService;
        }

        public async Task<PositionModel> Handle(GetPositionQuery request, CancellationToken cancellationToken)
        {
            var entity = _positionService.FindById(request.Id);

            return await entity
                .ProjectTo<PositionModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        }
    }
}
