using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using MediatR;
using Service.Interfaces;
using SmsIrRestful;

namespace Application.CommandQueries.Sms.Query.GetAllSended
{
    public class GetAllSendedQuery : Pagination, IRequest<PagedList<List<SentMessage>>>, ISearchableRequest, IQuery
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }

    public class GetAllSendedQueryHandler : IRequestHandler<GetAllSendedQuery, PagedList<List<SentMessage>>>
    {
        private readonly ISmsService _smsService;
        private readonly IMapper _mapper;

        public GetAllSendedQueryHandler(IMapper mapper, ISmsService smsService)
        {
            _mapper = mapper;
            _smsService = smsService;
        }

        public async Task<PagedList<List<SentMessage>>> Handle(GetAllSendedQuery request, CancellationToken cancellationToken)
        {
            return new PagedList<List<SentMessage>>()
            {
                Data = _smsService.GetSendedMessagesAsync(request.StartDate, request.EndDate, request.Paginator())
                    .ToList(),
                TotalCount = 0
            };
        }
    }
}
