using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using Infrastructure.Mappings;
using MediatR;
using Service.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Application.CommandQueries.Sms.Model;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using Infrastructure.ConfigurationAccessor;
using Infrastructure.CurrentUserAccessor;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using Infrastructure.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Office.Interop.Excel;
using SmsIrRestful;
using Range = Microsoft.Office.Interop.Excel.Range;
using Excel = Microsoft.Office.Interop.Excel;

namespace Application.CommandQueries.Sms.Command.Create
{
    public class CreateSendSmsCommand : CommandBase, IRequest<int>, IMapFrom<CreateSendSmsCommand>, ICommand
    {
        public bool DontSend { get; set; } = false;
        public int Reputation { get; set; }
        public string Title { get; set; } = default!;
        public int HeadLineNumberBaseId { get; set; } = default!;
        public int FeedbackNumber { get; set; } = default!;
        public DateTime EndDateTime { get; set; } = default!;
        public string? Descriptions { get; set; }
        public string FileAttachmentReletiveaddress { get; set; }
        public string Message { get; set; }
        public ICollection<Condition> Conditions { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateSendSmsCommand, Domain.Entities.OutgoingAdvertismentMessage>()
                .IgnoreAllNonExisting();
            profile.CreateMap<CreateSendSmsCommand, Domain.Entities.Advertisement>()
                .IgnoreAllNonExisting();
        }
    }

    public class CreateSendSmsCommandHandler : IRequestHandler<CreateSendSmsCommand, int>
    {
        private readonly IMapper _mapper;
        private readonly ISmsService _smsService;
        private readonly IRepository _repository;
        private readonly IBaseValueService _baseValueService;
        private readonly IAdvertisementService _advertisementSourceService;
        private readonly IOutgoingAdvertismentMessageService _outgoingAdvertismentMessageService;
        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly IConfigurationAccessor _configurationAccessor;
        private readonly IPersonService _personService;
        public CreateSendSmsCommandHandler(IMapper mapper, ISmsService smsService, IRepository repository, IAdvertisementService advertisementSourceService, IOutgoingAdvertismentMessageService outgoingAdvertismentMessageService, ICurrentUserAccessor currentUserAccessor, IConfigurationAccessor configurationAccessor, IPersonService personService, IBaseValueService baseValueService)
        {
            _mapper = mapper;
            _smsService = smsService;
            _repository = repository;
            _advertisementSourceService = advertisementSourceService;
            _outgoingAdvertismentMessageService = outgoingAdvertismentMessageService;
            _currentUserAccessor = currentUserAccessor;
            _configurationAccessor = configurationAccessor;
            _personService = personService;
            _baseValueService = baseValueService;
        }


        public async Task<int> Handle(CreateSendSmsCommand request, CancellationToken cancellationToken)
        {
            var numbers = new List<string>();
            if (!string.IsNullOrEmpty(request.FileAttachmentReletiveaddress))
            {
                var sourcePath = Path.Combine(_configurationAccessor.GetIoPaths().Root, request.FileAttachmentReletiveaddress.Remove(0, 7));
                sourcePath = sourcePath.Replace('/', '\\');
                var excel = new Excel.Application { Visible = false, DisplayAlerts = false };

                var p = Directory.GetCurrentDirectory();
                var h = Path.Combine(p, sourcePath);
                var workBook = excel.Workbooks.Open(h, 0, true, 5, "", "", true, XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                var workSheet = (Worksheet)workBook.Sheets[1];

                var cellRange = workSheet.UsedRange;
                for (var i = 1; i <= cellRange.Rows.Count; i++)
                {
                    var cellRangetemp = ((Range)cellRange.Cells[i, 1]).Value;
                    numbers.Add(cellRangetemp.ToString());
                }
            }
            else
            {
                numbers = await _personService
                          .GetAll()
                          .ProjectTo<PersonToSendModel>(_mapper.ConfigurationProvider)
                          .MakeStringSearchQuery(request.Conditions)
                          .Where(x => x.Phone != null && x.Phone.Trim() != "")
                          .Select(x => x.Phone)
                          .ToListAsync(cancellationToken)
                      ;
            }


            if (numbers.Count == 0)
            {
                return 0;
            }

            var inputeAdvertisment = _mapper.Map<Domain.Entities.Advertisement>(request);
            inputeAdvertisment.StartDateTime = DateTime.Now;
            inputeAdvertisment.HostName = "پنل پیامکی";
            inputeAdvertisment.AdvertisementSourceBaseId = (await _baseValueService.GetAllByUniqueName("sms").FirstOrDefaultAsync(cancellationToken)).Id;
            inputeAdvertisment.Reputation = numbers.Count;
            inputeAdvertisment.AdvertisementTypeBaseId = (await _baseValueService.GetAllByUniqueName("text").FirstOrDefaultAsync(cancellationToken)).Id;

            var headLineNumber = (await _baseValueService.FindById(request.HeadLineNumberBaseId).FirstOrDefaultAsync(cancellationToken)).Value;

            SentSMSLog2[] sendedLogs = null;
            if (request.DontSend)
            {
                inputeAdvertisment.Reputation = request.Reputation;
            }
            else
            {
                sendedLogs = _smsService.Send(request.Message, numbers, headLineNumber);
                inputeAdvertisment.Reputation = sendedLogs.Length;
            }

            var advertisment = await _advertisementSourceService.Add(inputeAdvertisment);
            if (sendedLogs != null)
            {
                foreach (var sentSmsLog2 in sendedLogs)
                {
                    await _outgoingAdvertismentMessageService.Add(new OutgoingAdvertismentMessage()
                    { Reciver = sentSmsLog2.MobileNo, AdvertismentSource = advertisment.Entity });
                }
            }

            if (await _repository.SaveChangesAsync(cancellationToken) > 0)
            {
                return sendedLogs?.Length ?? request.Reputation;
            }

            return 0;
        }
    }
}
