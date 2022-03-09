using Application.CommandQueries.Unit.Model;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Interfaces;


namespace Application.CommandQueries.Unit.Query.Get
{
    public class GetUnitQuery : IRequest<UnitModel>, IQuery
    {
        public int Id { get; set; }
    }

    public class GetUnitQueryHandler : IRequestHandler<GetUnitQuery, UnitModel>
    {
        private readonly IUnitService _unitService;
        private readonly IMapper _mapper;

        public GetUnitQueryHandler(IMapper mapper, IUnitService unitService)
        {
            _mapper = mapper;
            _unitService = unitService;
        }

        public async Task<UnitModel> Handle(GetUnitQuery request, CancellationToken cancellationToken)
        {
            var entity = _unitService.FindById(request.Id);

            return await entity
                .ProjectTo<UnitModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        }
    }
}
