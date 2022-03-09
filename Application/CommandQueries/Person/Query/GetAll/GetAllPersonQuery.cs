using System.Collections.Generic;
 
using Application.CommandQueries.Person.Model;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Infrastructure.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Interfaces;
using Infrastructure.Utilities;
using Persistence.SqlServer.QueryProvider;

namespace Application.CommandQueries.Person.Query.GetAll
{
    public class GetAllPersonQuery : Pagination, IRequest<PagedList<List<PersonModel>>>, ISearchableRequest, IQuery
    {
        public Condition[] Conditions { get; set; }

    }

    public class GetAllPersonQueryHandler : IRequestHandler<GetAllPersonQuery, PagedList<List<PersonModel>>>
    {
        private readonly IPersonService _personService;
        private readonly IMapper _mapper;

        public GetAllPersonQueryHandler(IMapper mapper, IPersonService personService)
        {
            _mapper = mapper;
            _personService = personService;
        }

        public async Task<PagedList<List<PersonModel>>> Handle(GetAllPersonQuery request, CancellationToken cancellationToken)
        {
            var entities = _personService
                    .GetAll()
                    .ProjectTo<PersonModel>(_mapper.ConfigurationProvider)
                    .MakeStringSearchQuery(request.Conditions)
                    .OrderByMultipleColumns(request.OrderByProperty)
                ;

            return new PagedList<List<PersonModel>>()
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
