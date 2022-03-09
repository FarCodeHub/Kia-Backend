using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.CommandQueries.CountryDivision.Model;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.SqlServer.QueryProvider;

namespace Application.CommandQueries.CountryDivision.Query.GetAllCitiesQuery
{
    public class GetAllCitiesQuery : Pagination, IRequest<List<CountryDivisionModel>>, ISearchableRequest, IQuery
    {
        public int StateId { get; set; }
    }

    public class GetAllCitiesQueryHandler : IRequestHandler<GetAllCitiesQuery, List<CountryDivisionModel>>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public GetAllCitiesQueryHandler(IRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<List<CountryDivisionModel>> Handle(GetAllCitiesQuery request, CancellationToken cancellationToken)
        {
            if (request.StateId == default)
            {
                return await _repository
                    .GetAll<Domain.Entities.CountryDivision>()
                    .ProjectTo<CountryDivisionModel>(_mapper.ConfigurationProvider)
                    .OrderByMultipleColumns(request.OrderByProperty)
                    .ToListAsync(cancellationToken);
            }
            else
            {
                return await _repository
                    .GetAll<Domain.Entities.CountryDivision>(c =>
                        c.ConditionExpression(x => x.ParentId.Equals(request.StateId)))
                      .ProjectTo<CountryDivisionModel>(_mapper.ConfigurationProvider)
                    .OrderByMultipleColumns(request.OrderByProperty)
                    .ToListAsync(cancellationToken);
            }
        }
    }
}
