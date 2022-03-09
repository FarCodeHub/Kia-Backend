using Application.Common.Interfaces;
using Application.$fileinputname$.Model;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Infrastructure.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.SqlServer;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Model;

namespace $rootnamespace$.$fileinputname$.Query.GetAll
{
    public class $safeitemname$ : Pagination,IRequest<ServiceResult>, ISearchableRequest, IQuery
    {
      
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
            .GetAll<Domain.Entities.$fileinputname$>(c => 
                 c.Paginate(new Pagination()
                {
                    Skip = request.Skip,
                    Take = request.Take,
                    OrderByProperty = request.OrderByProperty,
                    SortDirection = request.SortDirection
                }))
            .ProjectTo<$fileinputname$Model>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken));
         }
    }
}
    