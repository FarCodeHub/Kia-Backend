using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.CommandQueries.BaseValue.Model;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.SqlServer.QueryProvider;
using Service.Interfaces;

namespace Application.CommandQueries.BaseValue.Query.GetAllByBaseValueTypeId
{
    public class GetAllByBaseValueTypeIdQuery : Pagination, IRequest<PagedList<List<BaseValueModel>>>, ISearchableRequest, IQuery
    {
        public int BaseValueTypeId { get; set; }
    }

    public class GetAllByBaseValueTypeIdQueryHandler : IRequestHandler<GetAllByBaseValueTypeIdQuery, PagedList<List<BaseValueModel>>>
    {
        private readonly IBaseValueService _baseValueService;
        private readonly IMapper _mapper;

        public GetAllByBaseValueTypeIdQueryHandler(IMapper mapper, IBaseValueService baseValueService)
        {
            _mapper = mapper;
            _baseValueService = baseValueService;
        }

        public async Task<PagedList<List<BaseValueModel>>> Handle(GetAllByBaseValueTypeIdQuery request, CancellationToken cancellationToken)
        {
            var entities = _baseValueService.GetAllByBaseValueTypeId(request.BaseValueTypeId)
                    .ProjectTo<BaseValueModel>(_mapper.ConfigurationProvider)
                    .OrderByMultipleColumns(request.OrderByProperty)
                ;

            return new PagedList<List<BaseValueModel>>()
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