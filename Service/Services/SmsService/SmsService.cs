using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.ConfigurationAccessor;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces;
using Service.Models;
using Service.Services.BaseValue;
using ServiceStack;
using SmsIrRestful;

namespace Service.Services.SmsService
{

    public class SmsService : ISmsService
    {
        private readonly IConfigurationAccessor _configurationAccessor;
        private readonly SmsIrRestful.Token _token = new SmsIrRestful.Token();
        private IRepository _repository;
        private readonly IMapper _mapper;
        private readonly IPersonService _personService;
        private readonly ICustomerService _customerService;
        private readonly ICommunicationService _communicationService;
        private readonly IAdvertisementService _advertisementService;
        private readonly ICaseService _caseService;
        private readonly IEmployeeService _employeeService;
        private readonly ITaskService _taskService;
        private readonly IConfigurationService _configurationService;
        private readonly TaskResultsAndTypesSingleton _taskResultsAndTypesSingleton;
        public SmsService(IConfigurationAccessor configurationAccessor, IRepository repository, IMapper mapper, IPersonService personService, ICustomerService customerService, ICommunicationService communicationService, IAdvertisementService advertisementService, ICaseService caseService, IEmployeeService employeeService, ITaskService taskService, IConfigurationService configurationService)
        {
            _configurationAccessor = configurationAccessor;
            _repository = repository;
            _mapper = mapper;
            _personService = personService;
            _customerService = customerService;
            _communicationService = communicationService;
            _advertisementService = advertisementService;
            _caseService = caseService;
            _employeeService = employeeService;
            _taskService = taskService;
            _configurationService = configurationService;
            _taskResultsAndTypesSingleton = TaskResultsAndTypesSingleton.Instance(_repository);
        }

        public IRepository Repository { get => _repository; set => _repository = value; }

        private string Token()
        {
            var token = _token.GetToken(_configurationAccessor.GetSmsConfiguration().Apikey, _configurationAccessor.GetSmsConfiguration().Secret);

            return token;
        }
        public SentSMSLog2[] Send(string message, IList<string> numbers, string headLineNumber)
        {
            IList<string> currectNumbers = new List<string>();
            foreach (var number in numbers)
            {
                var n = number;
                if (!n.StartsWith('0'))
                {
                    n = $"0{number}";
                }

                if (n.Length < 11)
                {
                    continue;
                }
                currectNumbers.Add(n);
            }
           

            var messageSendObject = new MessageSendObject()
            {
                Messages = new List<string> { message }.ToArray(),
                MobileNumbers = currectNumbers.ToArray(),
                LineNumber = headLineNumber,
                SendDateTime = null,
                CanContinueInCaseOfError = true
            };

            var messageSendResponseObject = new MessageSend().Send(Token(), messageSendObject);

            if (messageSendResponseObject.IsSuccessful)
            {
                return messageSendResponseObject.Ids;
            }
            else
            {

            }

            return null;
        }


        public SentMessage[] GetSendedMessagesAsync(string startDate, string endDate, Pagination pagination)
        {
            var sentMessageResponseByDate = new MessageSend().GetByDate(Token(), startDate, endDate, 50, pagination.PageIndex - 1);

            if (sentMessageResponseByDate.IsSuccessful)
            {
                return sentMessageResponseByDate.Messages;
            }
            else
            {

            }

            return null;
        }

        public async Task<List<SignalCommunicationInformationModel>> CheckReceivedMessages()
        {
            var lastReadedSmsId = (await _configurationService.GetByKey("LastReadedSmsId").FirstOrDefaultAsync()).Value;
            var receivedMessages = new ReceiveMessage().GetByLastMessageID(Token(), int.Parse(lastReadedSmsId));

            var callInformationModels = new List<SignalCommunicationInformationModel>();

            if (receivedMessages.IsSuccessful)
            {
                var receivedList = receivedMessages.Messages.ToList();

                foreach (var received in receivedList)
                {
                    if (int.TryParse(received.SMSMessageBody, out int feedback) is false) continue;


                    var customer = await _customerService.GetAll().FirstOrDefaultAsync(x =>
                        received.MobileNo.Contains(x.Person.Phone1) ||
                        received.MobileNo.Contains(x.Person.Phone2) ||
                        received.MobileNo.Contains(x.Person.Phone3));




                    var casee = await _repository.GetQuery<Domain.Entities.Case>()
                        .Include(x => x.Customer)
                        .Include(x => x.Customer.Person)
                        .Include(x => x.Consultant)
                        .ThenInclude(x => x.Operator)
                        .FirstOrDefaultAsync(x => x.CustomerId == customer.Id);

                    var employee = casee?.Consultant;
                    if (casee is null)
                    {
                        customer = (await _customerService.Add(new Domain.Entities.Customer(),
                            new Domain.Entities.Person() { Phone1 = received.MobileNo })).Entity;

                        employee = _employeeService.GetAll()
                            .Where(x => x.UnitPosition.Position.UniqueName == "presenter")
                            .OrderBy(x => Guid.NewGuid())
                            .Include(x => x.Person)
                            .Include(x => x.Operator)
                            .FirstOrDefault();

                        casee = (await _caseService.Add(
                            new Domain.Entities.Case() { Customer = customer, Consultant = employee })).Entity;
                    }

                    var advertisment = await _advertisementService.FindByFeedBackNumber(feedback)
                        .FirstOrDefaultAsync();

                    var communication = (await _communicationService.Add(new Domain.Entities.Communication()
                    {
                        Customer = customer,
                        AdvertismentId = advertisment.Id,
                        TypeBaseId = _taskResultsAndTypesSingleton.IncomSms,
                        CustomerConnectedNumber = received.MobileNo,
                        Employee = employee,
                        EmployeeId = employee.Id,
                        CustomerId = customer.Id,
                        SmsUniqueNumber = received.ID.ToString()
                    })).Entity;

                    var newTask = (await _taskService.Add(new Domain.Entities.Task()
                    {
                        Employee = employee,
                        Customer = customer,
                        CustomerId = customer.Id,
                        EmployeeId = employee.Id,
                        Communication = communication,
                        Case = casee,
                        DuoAt = DateTime.Now,
                        TypeBaseId = _taskResultsAndTypesSingleton.Follow,
                    })).Entity;

                    await _configurationService.Update(new Domain.Entities.Configuration()
                    {
                        Key = "LastReadedSmsId",
                        Value = received.ID.ToString()
                    });

                    await _repository.SaveChangesAsync();

                    callInformationModels.Add(new SignalCommunicationInformationModel(Guid.NewGuid().ToString())
                    {
                        EmployeeId = employee.Id,
                        TaskId = newTask.Id,
                        OperatorQueueNumber = employee.Operator.QueueNumber,
                        CommunicationId = communication.Id,
                        UniqueNumber = received.ID.ToString(),
                        CustomerConnectedNumber = received.MobileNo,
                        CustomerId = customer.Id,
                        StatusTitle = "IncomeSms",
                        Communication = communication,
                        Task = newTask
                    });
                }

            }

            return callInformationModels;
        }

        public ReceivedMessages[] GetReceivedMessages(string startDate, string endDate, Pagination pagination)
        {
            var receivedMessages = new ReceiveMessage().GetByDate(Token(), startDate, endDate, 50, pagination.PageIndex - 1);

            if (receivedMessages.IsSuccessful)
            {
                return receivedMessages.Messages;

            }
            else
            {

            }

            return Array.Empty<ReceivedMessages>();
        }

        public async Task<List<ReceivedMessages>> GetReceivedMessages()
        {
            var lastReadedSmsId = (await _configurationService.GetByKey("LastReadedSmsId").FirstOrDefaultAsync()).Value;
            var receivedMessages = new ReceiveMessage().GetByLastMessageID(Token(), int.Parse(lastReadedSmsId) + 1);

            if (receivedMessages.IsSuccessful)
            {
                return receivedMessages.Messages?.ToList();
            }
            else
            {

            }

            return new List<ReceivedMessages>();
        }


        public bool SendQuickMessage(string number, QuickMessageType quickMessageType, IQuickMessage quickMessage)
        {
            QuickMessagePreparer(quickMessageType, quickMessage);
            var ultraFastSend = new UltraFastSend()
            {
                Mobile = long.Parse(number),
                ParameterArray = QuickMessagePreparer(quickMessageType, quickMessage).ToArray()
            };


            if (quickMessageType == QuickMessageType.Appointment)
            {
                ultraFastSend.TemplateId = 58602;
            }
            else if (quickMessageType == QuickMessageType.OperatorAssignment)
            {
                ultraFastSend.TemplateId = 58603;
            }
            else if (quickMessageType == QuickMessageType.ProjectInfo)
            {
                ultraFastSend.TemplateId = 59253;
            }


            var ultraFastSendRespone = new UltraFast().Send(Token(), ultraFastSend);

            if (ultraFastSendRespone.IsSuccessful)
            {
                return true;
            }
            else
            {

            }

            return false;
        }

        public List<UltraFastParameters> QuickMessagePreparer(QuickMessageType quickMessageType, IQuickMessage quickMessage)
        {
            var message = new List<UltraFastParameters>();
            switch (quickMessageType)
            {
                case QuickMessageType.OperatorAssignment:
                    var operatorAssignmentQuickMessage = quickMessage as OperatorAssignmentQuickMessage;
                    message.Add(new UltraFastParameters() { Parameter = "employee", ParameterValue = operatorAssignmentQuickMessage?.OperatorFullName });
                    break;
                case QuickMessageType.ProjectInfo:
                    var projectInfoQuickMessage = quickMessage as ProjectInfoQuickMessage;
                    message.Add(new UltraFastParameters() { Parameter = "customer", ParameterValue = projectInfoQuickMessage?.CustomerFullName });
                    message.Add(new UltraFastParameters() { Parameter = "title", ParameterValue = projectInfoQuickMessage?.ProjectTitle });
                    message.Add(new UltraFastParameters() { Parameter = "link", ParameterValue = projectInfoQuickMessage?.ProjectFileLink });
                    break;
                case QuickMessageType.Appointment:
                    var appointmentQuickMessage = quickMessage as AppointmentQuickMessage;
                    var appintmentType = "";
                    if (appointmentQuickMessage?.AppointmentType == Appointmenttype.Visit)
                    {
                        appintmentType = "بازدید زمین";
                        message.Add(new UltraFastParameters()
                        {
                            Parameter = "link",
                            ParameterValue = _configurationAccessor
                                .LandingConfiguration().Url + "p" + appointmentQuickMessage.GeoLocationLink
                        });
                    }
                    else if (appointmentQuickMessage?.AppointmentType == Appointmenttype.Consult)
                    {
                        appintmentType = "مشاوره حضوری";
                        message.Add(new UltraFastParameters()
                        {
                            Parameter = "link",
                            ParameterValue = _configurationAccessor
                                .LandingConfiguration().Url + "b" + appointmentQuickMessage.GeoLocationLink
                        });

                    }
                    message.Add(new UltraFastParameters() { Parameter = "cutomer", ParameterValue = appointmentQuickMessage.CustomerFullName });
                    message.Add(new UltraFastParameters() { Parameter = "appointmentType", ParameterValue = appintmentType });
                    message.Add(new UltraFastParameters() { Parameter = "date", ParameterValue = appointmentQuickMessage.PersianDate });
                    message.Add(new UltraFastParameters() { Parameter = "time", ParameterValue = appointmentQuickMessage.PersianTime });
                    message.Add(new UltraFastParameters() { Parameter = "employee", ParameterValue = appointmentQuickMessage.EmployeeFullName });
                    break;
            }

            return message;
        }
        public enum QuickMessageType
        {
            Appointment,
            OperatorAssignment,
            ProjectInfo
        }

        public enum Appointmenttype
        {
            Visit,
            Consult
        }


        public interface IQuickMessage
        {

        }

        public class OperatorAssignmentQuickMessage : IQuickMessage
        {
            public string OperatorFullName { get; set; }
        }

        public class ProjectInfoQuickMessage : IQuickMessage
        {
            public string CustomerFullName
            {
                get => CustomerFullName;
                set
                {
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        this.CustomerFullName = "مشتری";
                    }
                    else
                    {
                        this.CustomerFullName = value;
                    }
                }

            }
            public string ProjectTitle { get; set; }
            public string ProjectFileLink { get; set; }
        }

        public class AppointmentQuickMessage : IQuickMessage
        {
            private readonly PersianCalendar _persianCalendar = new PersianCalendar();
            public string PersianDate
            {
                get
                {
                    return $"{_persianCalendar.GetYear(DateTime)}/{_persianCalendar.GetMonth(DateTime)}/{_persianCalendar.GetDayOfMonth(DateTime)}";
                }
            }

            public string PersianTime
            {
                get
                {
                    return $"{_persianCalendar.GetHour(DateTime)}";
                }
            }

            public string CustomerFullName
            {
                get => CustomerFullName;
                set
                {
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        this.CustomerFullName = "مشتری";
                    }
                    else
                    {
                        this.CustomerFullName = value;
                    }
                }

            }
            public Appointmenttype AppointmentType { get; set; }
            public DateTime DateTime { get; set; }
            public string EmployeeFullName { get; set; }
            public string GeoLocationLink { get; set; }
        }
    }
}