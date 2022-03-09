using System.Threading;
using System.Threading.Tasks;
using Application.CommandQueries.AdvertismentSource.Model;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Infrastructure.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces;

namespace Application.CommandQueries.AdvertismentSource.Query.Get
{
    public class GetAdvertisementSourceQuery : IRequest<AdvertisementSourceModel>, IQuery
    {
        public int Id { get; set; }
    }

    public class GetAdvertisementSourceQueryHandler : IRequestHandler<GetAdvertisementSourceQuery, AdvertisementSourceModel>
    {
        private readonly IAdvertisementService _advertisementSourceService;
        private readonly IMapper _mapper;

        public GetAdvertisementSourceQueryHandler(IMapper mapper, IAdvertisementService advertisementSourceService)
        {
            _mapper = mapper;
            _advertisementSourceService = advertisementSourceService;
        }

        public async Task<AdvertisementSourceModel> Handle(GetAdvertisementSourceQuery request, CancellationToken cancellationToken)
        {
            var entity = _advertisementSourceService.FindById(request.Id);

            return await entity
                .ProjectTo<AdvertisementSourceModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
