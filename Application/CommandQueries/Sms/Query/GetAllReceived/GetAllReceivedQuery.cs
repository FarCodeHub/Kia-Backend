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

namespace Application.CommandQueries.Sms.Query.GetAllReceived
{
    public class GetAllReceivedQuery : Pagination, IRequest<PagedList<List<ReceivedMessages>>>, ISearchableRequest, IQuery
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }

    public class GetAllReceivedQueryHandler : IRequestHandler<GetAllReceivedQuery, PagedList<List<ReceivedMessages>>>
    {
        private readonly ISmsService _smsService;
        private readonly IMapper _mapper;

        public GetAllReceivedQueryHandler(IMapper mapper, ISmsService smsService)
        {
            _mapper = mapper;
            _smsService = smsService;
        }

        public async Task<PagedList<List<ReceivedMessages>>> Handle(GetAllReceivedQuery request, CancellationToken cancellationToken)
        {
            //_smsService.SendQuickMessage("09354105810", SmsService.QuickMessageType.Appointment,
            //    new SmsService.AppointmentQuickMessage() { AppointmentType = SmsService.Appointmenttype.Visit, DateTime = DateTime.Now, EmployeeFullName = "پدرام رنگچیان", CustomerFullName = "دانیال درخشانی", GeoLocationLink = "15" });


            //_smsService.SendQuickMessage("09354105810", SmsService.QuickMessageType.OperatorAssignment,
            //    new SmsService.OperatorAssignmentQuickMessage() {OperatorFullName = "پدرام رنگچیان" });

            return new PagedList<List<ReceivedMessages>>()
            {
                Data = _smsService
                    .GetReceivedMessages(request.StartDate, request.EndDate, request.Paginator())
                    .ToList(),
                TotalCount = 0
            };
        }
    }
}
