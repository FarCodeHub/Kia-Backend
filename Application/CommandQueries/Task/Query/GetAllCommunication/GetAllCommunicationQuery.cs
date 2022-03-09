using System.Collections.Generic;
using System.Threading;
using Application.CommandQueries.Task.Model;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using Infrastructure.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.SqlServer.QueryProvider;
using Service.Interfaces;

namespace Application.CommandQueries.Task.Query.GetAllCommunication
{
    public class GetAllCommunicationQuery : Pagination, IRequest<PagedList<List<CommunicationModel>>>, ISearchableRequest, IQuery
    {
        public Condition[] Conditions { get; set; }
    }

    public class GetAllCommunicationQueryHandler : IRequestHandler<GetAllCommunicationQuery, PagedList<List<CommunicationModel>>>
    {
        private readonly ITaskService _sessionService;
        private readonly IMapper _mapper;
        private readonly ICommunicationService _communicationService;

        public GetAllCommunicationQueryHandler(IMapper mapper, ITaskService sessionService, ICommunicationService communicationService)
        {
            _mapper = mapper;
            _sessionService = sessionService;
            _communicationService = communicationService;
        }

        public async System.Threading.Tasks.Task<PagedList<List<CommunicationModel>>> Handle(GetAllCommunicationQuery request, CancellationToken cancellationToken)
        {
            var entities = _communicationService
                    .GetAll()
                    .ProjectTo<CommunicationModel>(_mapper.ConfigurationProvider)
                    .MakeStringSearchQuery(request.Conditions)
                    .OrderByMultipleColumns(request.OrderByProperty)
                ;

            return new PagedList<List<CommunicationModel>>()
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