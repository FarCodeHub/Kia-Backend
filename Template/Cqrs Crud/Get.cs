using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.$fileinputname$.Model;
using Persistence.SqlServer;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Application.Common.Model;


namespace $rootnamespace$.$fileinputname$.Query.Get
{
    public class $safeitemname$ : IRequest<ServiceResult>, IQuery
    {
        public object Id {get;set;}
    }

    public class $safeitemname$Handler : IRequestHandler<$safeitemname$, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public $safeitemname$Handler(IRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult> Handle($safeitemname$ request, CancellationToken cancellationToken)
        {
            return ServiceResult.Set(await _repository
                .Find<Domain.Entities.$fileinputname$>(c 
            => c.ObjectId(request.Id))
            .ProjectTo<$fileinputname$Model>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken));
        }
    }
}
    