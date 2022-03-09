using Application.$fileinputname$.Model;
using Application.Common.Common.Interfaces;
using Application.Common.Common.Model;
using Infrastructure.Mappings;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Infrastructure.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Service.Interfaces;

namespace $rootnamespace$.$fileinputname$.Query.GetAll
{
    public class GetAll$fileinputname$Query : Pagination, IRequest<ServiceResult>, ISearchableRequest, IQuery
    {

    }

    public class GetAll$fileinputname$QueryHandler : IRequestHandler<GetAll$fileinputname$Query, ServiceResult>
    {
        private readonly I$fileinputname$Service _$fileinputname$Service;
        private readonly IMapper _mapper;

        public GetAll$fileinputname$QueryHandler(IMapper mapper, I$fileinputname$Service $fileinputname$Service)
        {
            _mapper = mapper;
            _$fileinputname$Service = $fileinputname$Service;
        }

        public async Task<ServiceResult> Handle(GetAll$fileinputname$Query request, CancellationToken cancellationToken)
        {
            var entities = _$fileinputname$Service.GetAll(new Pagination()
            {
                Skip = request.Skip,
                Take = request.Take,
                OrderByProperty = request.OrderByProperty,
                SortDirection = request.SortDirection
            });

            return ServiceResult.Set(await entities
            .ProjectTo<$fileinputname$Model>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken));
        }
    }
}
