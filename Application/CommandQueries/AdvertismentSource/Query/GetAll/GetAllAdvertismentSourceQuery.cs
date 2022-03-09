using System.Collections.Generic;
 
using System.Threading;
using System.Threading.Tasks;
using Application.CommandQueries.AdvertismentSource.Model;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using Infrastructure.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.SqlServer.QueryProvider;
using Service.Interfaces;

namespace Application.CommandQueries.AdvertismentSource.Query.GetAll
{
    public class GetAllAdvertisementSourceQuery : Pagination, IRequest<PagedList<List<AdvertisementSourceModel>>>, ISearchableRequest, IQuery
    {
        public Condition[] Conditions { get; set; }

    }

    public class GetAllAdvertisementSourceQueryHandler : IRequestHandler<GetAllAdvertisementSourceQuery, PagedList<List<AdvertisementSourceModel>>>
    {
        private readonly IAdvertisementService _advertisementSourceService;
        private readonly IMapper _mapper;

        public GetAllAdvertisementSourceQueryHandler(IMapper mapper, IAdvertisementService advertisementSourceService)
        {
            _mapper = mapper;
            _advertisementSourceService = advertisementSourceService;
        }

        public async Task<PagedList<List<AdvertisementSourceModel>>> Handle(GetAllAdvertisementSourceQuery request, CancellationToken cancellationToken)
        {
            var entities = _advertisementSourceService
                .GetAll()
                .ProjectTo<AdvertisementSourceModel>(_mapper.ConfigurationProvider)
                .MakeStringSearchQuery(request.Conditions)
                .OrderByMultipleColumns(request.OrderByProperty);

            return new PagedList<List<AdvertisementSourceModel>>()
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
