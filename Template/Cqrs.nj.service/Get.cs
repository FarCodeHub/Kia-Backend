using Application.$fileinputname$.Model;
using Application.Common.Common.Interfaces;
using Application.Common.Common.Model;
using Infrastructure.Mappings;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces;


namespace Application.$fileinputname$.Query.Get
{
    public class Get$fileinputname$Query : IRequest<ServiceResult>, IQuery
    {
        public int Id { get; set; }
    }

    public class Get$fileinputname$QueryHandler : IRequestHandler<Get$fileinputname$Query, ServiceResult>
    {
        private readonly I$fileinputname$Service _$fileinputname$Service;
        private readonly IMapper _mapper;

        public Get$fileinputname$QueryHandler(IMapper mapper, I$fileinputname$Service $fileinputname$Service)
        {
            _mapper = mapper;
            _$fileinputname$Service = $fileinputname$Service;
        }

        public async Task<ServiceResult> Handle(Get$fileinputname$Query request, CancellationToken cancellationToken)
        {
            var entity = _$fileinputname$Service.FindById(request.Id);

            return ServiceResult.Set(await entity
                .ProjectTo<$fileinputname$Model>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken));
        }
    }
}
