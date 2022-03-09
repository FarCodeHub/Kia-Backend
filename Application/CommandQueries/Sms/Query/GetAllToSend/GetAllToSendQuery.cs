using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.CommandQueries.Sms.Model;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using Infrastructure.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.SqlServer.QueryProvider;
using Service.Interfaces;

namespace Application.CommandQueries.Sms.Query.GetAllToSend
{
    public class GetAllToSendQuery : Pagination, IRequest<PagedList<List<PersonToSendModel>>>, ISearchableRequest, IQuery
    {
        public Condition[] Conditions { get; set; }

    }

    public class GetAllToSendQueryHandler : IRequestHandler<GetAllToSendQuery, PagedList<List<PersonToSendModel>>>
    {
        private readonly IPersonService _personService;
        private readonly IMapper _mapper;

        public GetAllToSendQueryHandler(IMapper mapper, IPersonService personService)
        {
            _mapper = mapper;
            _personService = personService;
        }

        public async Task<PagedList<List<PersonToSendModel>>> Handle(GetAllToSendQuery request, CancellationToken cancellationToken)
        {
            var entities = _personService
                    .GetAll()
                    .ProjectTo<PersonToSendModel>(_mapper.ConfigurationProvider)
                    .MakeStringSearchQuery(request.Conditions)
                    .OrderByMultipleColumns(request.OrderByProperty)
                ;

            return new PagedList<List<PersonToSendModel>>()
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
