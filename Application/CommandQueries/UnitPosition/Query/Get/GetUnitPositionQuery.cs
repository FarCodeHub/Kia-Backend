using Application.CommandQueries.UnitPosition.Model;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Interfaces;


namespace Application.CommandQueries.UnitPosition.Query.Get
{
    public class GetUnitPositionQuery : IRequest<UnitPositionModel>, IQuery
    {
        public int Id { get; set; }
    }

    public class GetUnitPositionQueryHandler : IRequestHandler<GetUnitPositionQuery, UnitPositionModel>
    {
        private readonly IUnitPositionService _unitPositionService;
        private readonly IMapper _mapper;

        public GetUnitPositionQueryHandler(IMapper mapper, IUnitPositionService unitPositionService)
        {
            _mapper = mapper;
            _unitPositionService = unitPositionService;
        }

        public async Task<UnitPositionModel> Handle(GetUnitPositionQuery request, CancellationToken cancellationToken)
        {
            var entity = _unitPositionService.FindById(request.Id);

            return await entity
                .ProjectTo<UnitPositionModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        }
    }
}
