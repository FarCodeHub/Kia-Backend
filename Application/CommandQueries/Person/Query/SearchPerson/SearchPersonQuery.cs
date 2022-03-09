using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Threading;
using System.Threading.Tasks;
using Application.CommandQueries.Person.Model;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Infrastructure.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CommandQueries.Person.Query.SearchPerson
{
    public class SearchPersonQuery : IRequest<List<PersonModel>>, IQuery
    {
        public string Search { get; set; }
    }

    public class SearchPersonQueryHandler : IRequestHandler<SearchPersonQuery, List<PersonModel>>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public SearchPersonQueryHandler(IRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        [Obsolete]
        public async Task<List<PersonModel>> Handle(SearchPersonQuery request, CancellationToken cancellationToken)
        {
            return await _repository
                .GetAll<Domain.Entities.Person>(c
                    => c.ConditionExpression(x => x.NationalCode == request.Search ||
                                                  (x.FirstName + " " + x.LastName)
                                                  .Contains(EntityFunctions.AsUnicode(request.Search))))
                .ProjectTo<PersonModel>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
    }
}
