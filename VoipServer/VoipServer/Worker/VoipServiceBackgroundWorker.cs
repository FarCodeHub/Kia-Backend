using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.SignalR;
using AsterNET.Manager;
using AsterNET.Manager.Event;
using Domain.Entities;
using Infrastructure.Configurations.RedisConfigurations;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Service.Interfaces;
using Service.Models;
using Service.Services.BaseValue;
using Service.Services.SmsService;
using SmsIrRestful;
using VoipServer.Data.Context.MySql;
using VoipServer.Data.Entities;
using VoipServer.Hubs;
using VoipServer.Services;
using VoipServer.Utilities;
using Task = Domain.Entities.Task;

namespace VoipServer.Worker
{
    public interface IScopedProcessingService
    {
        System.Threading.Tasks.Task DoWorkAsync(CancellationToken stoppingToken);
    }

    public class VoipServiceBackgroundWorker : BackgroundService
    {
        #region props
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<VoipServiceBackgroundWorker> _logger;
        private readonly ManagerConnection _manager;
        private readonly IHubContext<VoipHub> _messageBrokerHubContext;
        private readonly TaskResultsAndTypesSingleton _taskResultsAndTypesSingleton;
        #endregion

        #region ctor

        public VoipServiceBackgroundWorker(ILogger<VoipServiceBackgroundWorker> logger,
            IHubContext<VoipHub> messageBrokerHubContext, IServiceProvider serviceProvider)
        {
            _manager = VoipCoreManager.Instance.Manager();
            _manager.Hangup += Hangup;
            _manager.NewState += NewState;
            _logger = logger;
            _messageBrokerHubContext = messageBrokerHubContext;
            _serviceProvider = serviceProvider;
            _taskResultsAndTypesSingleton = TaskResultsAndTypesSingleton.Instance(GetRepository());
        }
        #endregion


        protected override async System.Threading.Tasks.Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await SignalDelivery(cancellationToken: stoppingToken);

            await Connect(stoppingToken);

            await CheckQuitedQueue(stoppingToken);

            await CheckInboxSms(stoppingToken);
        }



        public async System.Threading.Tasks.Task SignalDelivery(CancellationToken cancellationToken)
        {
            async void Action()
            {
                while (true)
                {
                    var redisDataProvider = GetRedisDataProvider();

                    var noDeliveries = redisDataProvider.Redis().GetAllKeys();

                    foreach (var noDeliveryKey in noDeliveries)
                    {
                        if (!Guid.TryParse(noDeliveryKey, out var g)) continue;
                        var callInformationModel = redisDataProvider.Get<SignalCommunicationInformationModel>(noDeliveryKey);
                        if (callInformationModel is null)
                        {
                            continue;
                        }
                        await _messageBrokerHubContext.Clients.All.SendAsync(callInformationModel.OperatorQueueNumber, JsonConvert.SerializeObject(callInformationModel), cancellationToken);

                        _logger.Log2(JsonConvert.SerializeObject(callInformationModel), Utility.LogType.Warning);
                    }

                    Thread.Sleep(TimeSpan.FromSeconds(5));
                }
            }


            new System.Threading.Tasks.Task(Action).Start();
        }



        public async System.Threading.Tasks.Task CheckInboxSms(CancellationToken cancellationToken)
        {
            async void Action()
            {
                while (true)
                {
                    var repository = GetRepository();
                    var redisDataProvider = GetRedisDataProvider();
                    var smsService = GetSmsService();
                    var communicationService = GetCommunicationService();
                    var taskService = GetTaskService();
                    var advertisementService = GetAdvertisementService();
                    var employeeService = GetEmployeeService();
                    var caseService = GetCaseService();
                    var configurationService = GetConfigurationService();

                    caseService.Repository = repository;
                    configurationService.Repository = repository;
                    employeeService.Repository = repository;
                    smsService.Repository = repository;
                    communicationService.Repository = repository;
                    taskService.Repository = repository;
                    advertisementService.Repository = repository;


                    var messages = await smsService.GetReceivedMessages();

                    foreach (var received in messages ?? new List<ReceivedMessages>())
                    {
                        if (int.TryParse(received.SMSMessageBody, out var feedback) is false)
                        {
                            IsCustomerExist(received.MobileNo, out bool isNew);
                            if (isNew)
                            {
                                continue;
                            }
                        }

                        var @case = GetOrCreateCase(received.MobileNo, out var isNewCustomer);

                        var customer = @case.Customer;

                        var employee = @case?.Consultant;
                        if (employee == null)
                        {
                            employee = employeeService.GetAll()
                                .Where(x => x.UnitPosition.Position.UniqueName == "consultant")
                                .OrderBy(x => Guid.NewGuid())
                                .Include(x => x.Person)
                                .FirstOrDefault();

                            @case.ConsultantId = employee?.Id;
                            await caseService.Update(@case, CancellationToken.None);
                        }

                        if (employee is null) throw new Exception("consultant not found");

                        Advertisement advertisment = null;
                        if (feedback != 0)
                        {
                            advertisment = await advertisementService.FindByFeedBackNumber(feedback)
                               .FirstOrDefaultAsync(cancellationToken: cancellationToken);
                        }


                        var communication = (await communicationService.Add(new Domain.Entities.Communication()
                        {
                            AdvertismentId = advertisment?.Id,
                            TypeBaseId = _taskResultsAndTypesSingleton.IncomSms,
                            CustomerConnectedNumber = received.MobileNo,
                            EmployeeId = employee.Id,
                            CustomerId = customer.Id,
                            SmsUniqueNumber = received.ID.ToString()
                        })).Entity;

                        var newTask = (await taskService.Add(new Domain.Entities.Task()
                        {
                            CustomerId = customer.Id,
                            EmployeeId = employee.Id,
                            Communication = communication,
                            CommunicationId = communication.Id,
                            CaseId = @case.Id,
                            DuoAt = DateTime.Now,
                            TypeBaseId = _taskResultsAndTypesSingleton.AnswerSms,
                        })).Entity;

                        await configurationService.Update(new Domain.Entities.Configuration()
                        {
                            Key = "LastReadedSmsId",
                            Value = received.ID.ToString()
                        });

                        if (await repository.SaveChangesAsync(cancellationToken) > 0)
                        {
                            CheckAndChangeCustomerAndOperatorLink(received.MobileNo, employee.Operator.QueueNumber);

                            if (isNewCustomer)
                            {
                                smsService.SendQuickMessage(received.MobileNo,
                                    Service.Services.SmsService.SmsService.QuickMessageType.OperatorAssignment,
                                    new SmsService.OperatorAssignmentQuickMessage()
                                    {
                                        OperatorFullName = employee.Person.FirstName + " " + employee.Person.LastName
                                    });
                            }
                        }

                        var signalCommunicationInformationModel = new SignalCommunicationInformationModel(Guid.NewGuid().ToString())
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
                        };
                        await _messageBrokerHubContext.Clients.All.SendAsync(signalCommunicationInformationModel.OperatorQueueNumber,
                            JsonConvert.SerializeObject(signalCommunicationInformationModel), cancellationToken);

                        redisDataProvider.Update(signalCommunicationInformationModel.SignalId, signalCommunicationInformationModel, TimeSpan.FromHours(12));
                    }

                    Thread.Sleep(TimeSpan.FromMinutes(20));
                }
            }

            new System.Threading.Tasks.Task(Action).Start();
        }



        public async System.Threading.Tasks.Task Connect(CancellationToken cancellationToken)
        {
            try
            {
                _manager.Login();

                while (!_manager.IsConnected())
                {
                    _manager.Login();
                }

                _logger.LogInformation(@"-----Connected-----" + Environment.NewLine + "Asterisk version : " + _manager.Version);


                var report = new System.Threading.Tasks.Task(action: () =>
                {
                    while (true)
                    {
                        if (_manager.IsConnected())
                        {
                            _logger.Log2($"[{DateTime.Now:HH:mm:ss}] Server Report : Asterisk {_manager.Version} is running on background service", Utility.LogType.Information);
                        }
                        else
                        {
                            _logger.Log2($"[{DateTime.Now:HH:mm:ss}] Server Report : Asterisk {_manager.Version} is NOT running", Utility.LogType.Error);
                        }

                        Thread.Sleep(30000);
                    }
                });

                if (_manager.IsConnected())
                {
                    report.Start();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(@"-----Not connected-----" + Environment.NewLine + ex.Message);
                Console.Beep(1500, 2000);
            }
        }


        private void StartTask(string operatorNumber, string customerNumber)
        {
            var repository = GetRepository();
            var taskService = GetTaskService();
            var employeeService = GetEmployeeService();
            var customerService = GetCustomerService();
            var caseService = GetCaseService();

            taskService.Repository = repository;
            employeeService.Repository = repository;
            customerService.Repository = repository;
            caseService.Repository = repository;

            var emp = employeeService.GetAll().FirstOrDefault(x => x.Operator.ExtentionNumber == operatorNumber);
            var c = IsCustomerExist(customerNumber, out bool isNew);

            var t = taskService.GetAll().AsNoTracking().OrderBy(x => x.CreatedAt)
                .LastOrDefault(x => x.EmployeeId == emp.Id && x.CustomerId == c.Id && x.StartAt == null && x.EndAt == null);
            if (t != null) taskService.StartTask(t.Id).GetAwaiter().GetResult();
            repository.SaveChanges();
        }

        private void NewState(object sender, NewStateEvent e)
        {
            if (e.CallerIdNum == e.Connectedlinenum || e.Connectedlinenum is null || e.CallerIdNum == null)
            {
                return;
            }

            if (e.CallerIdNum.Length <= 4 && e.ChannelStateDesc == "Up")
            {
                var cn = Utility.SimplifiyPhoneNumber(e.Connectedlinenum); // customer
                var op = Utility.SimplifiyPhoneNumber(e.CallerIdNum); // operator
                StartTask(op, cn);
            }


            if (e.CallerIdNum?.Length <= 4 && e.Connectedlinenum?.Length <= 4)
            {
                return;
            }

            else if (e.ChannelState == "5" && e.ChannelStateDesc == "Ringing")
            {
                var repository = GetRepository();
                var redisDataProvider = GetRedisDataProvider();
                var communicationService = GetCommunicationService();
                var taskService = GetTaskService();
                var advertisementService = GetAdvertisementService();
                var employeeService = GetEmployeeService();
                var voipMySqlUnitOfWork = GetVoipMySqlUnitOfWork();
                var caseService = GetCaseService();

                caseService.Repository = repository;
                communicationService.Repository = repository;
                taskService.Repository = repository;
                advertisementService.Repository = repository;
                employeeService.Repository = repository;

                var signalId = Guid.NewGuid().ToString();
                string operatorQueueNumber;
                string customerNumber;
                var isIncoming = false;
                Customer customer = null;
                Employee employee = null;
                var communication = new Communication();

                if (e.CallerIdNum.Length > 4) // if more than 4 => outgoing else => incoming
                {
                    customerNumber = Utility.SimplifiyPhoneNumber(e.CallerIdNum); // dest
                    operatorQueueNumber = $"1{Utility.SimplifiyPhoneNumber(e.ConnectedLineName)}"; // queue

                    communication.TypeBaseId = _taskResultsAndTypesSingleton.OutgoingCall;
                }
                else
                {
                    operatorQueueNumber = $"1{Utility.SimplifiyPhoneNumber(e.CallerIdNum)}"; // queue
                    customerNumber = Utility.SimplifiyPhoneNumber(e.Connectedlinenum); // dest

                    communication.TypeBaseId = _taskResultsAndTypesSingleton.IncomingCall;
                    isIncoming = true;
                }

                communication.CustomerConnectedNumber = customerNumber;

                //_messageBrokerHubContext.Clients.All.SendAsync("statusChangedSignal",
                //        operatorQueueNumber)
                //    .GetAwaiter()
                //    .GetResult();

                var @case = GetOrCreateCase(Utility.SimplifiyPhoneNumber(customerNumber), out var isNewCustomer);
                customer = @case.Customer;


                employee = employeeService.GetAll().FirstOrDefault(x =>
                  x.Operator.QueueNumber == operatorQueueNumber && x.Operator.IsActive == true);

                if (employee is null)
                {
                    throw new Exception("employee not found");
                }

                @case.ConsultantId = employee.Id;
                caseService.Update(@case, CancellationToken.None).GetAwaiter().GetResult();

                if (isNewCustomer)
                {
                    var newestAdvertisment = advertisementService.GetAll()
                        .OrderBy(x => x.StartDateTime)
                        .FirstOrDefault(x => x.EndDateTime! <= DateTime.Now);

                    if (newestAdvertisment != null) communication.AdvertismentId = newestAdvertisment.Id;
                }
                else
                {
                    communication.AdvertismentId = communicationService.GetAll()
                        .OrderBy(x => x.CreatedAt)
                        .LastOrDefault(x => x.CustomerId == customer.Id)?.AdvertismentId ?? null;
                }

                communication.CustomerId = customer.Id;
                communication.EmployeeId = employee.Id;


                var uniqueId = voipMySqlUnitOfWork.Cel
                    .FirstOrDefault(x => x.uniqueid == e.UniqueId)
                    ?.linkedid;


                communication.VoipUniqueNumber = uniqueId;
                var insertedCommunication = communicationService.Add(communication).GetAwaiter().GetResult();

                var callInformation = new SignalCommunicationInformationModel(signalId)
                {
                    CustomerId = customer.Id,
                    EmployeeId = employee.Id,
                    Communication = insertedCommunication.Entity,
                    UniqueNumber = uniqueId,
                    CustomerConnectedNumber = customerNumber,
                    OperatorQueueNumber = operatorQueueNumber,
                    StatusTitle = e.ChannelStateDesc
                };

                if (isIncoming)
                {
                    Task task = new()
                    {
                        Status = 1,
                        EmployeeId = employee.Id,
                        CustomerId = customer.Id,
                        TypeBaseId = _taskResultsAndTypesSingleton.AnswerCall,
                        DuoAt = DateTime.Now,
                        Communication = insertedCommunication.Entity
                    };
                    callInformation.Task = taskService.Add(task).GetAwaiter().GetResult().Entity;
                }

                repository.SaveChanges();

                callInformation.TaskId = callInformation.Task?.Id ?? 0;
                callInformation.CommunicationId = callInformation.Communication?.Id ?? 0;

                callInformation.Task = null;
                callInformation.Communication = null;


                redisDataProvider.Update(callInformation.SignalId, callInformation, TimeSpan.FromHours(12));


                _messageBrokerHubContext.Clients.All.SendAsync(operatorQueueNumber,
                        JsonConvert.SerializeObject(callInformation))
                    .GetAwaiter()
                    .GetResult();

                _logger.LogInformation(JsonConvert.SerializeObject(callInformation));
                CheckAndChangeCustomerAndOperatorLink(customerNumber, operatorQueueNumber);
            }
        }


        private void CheckAndChangeCustomerAndOperatorLink(string customerNumber, string operatorQueueNumber)
        {
            var voipMySqlUnitOfWork = GetVoipMySqlUnitOfWork();

            crm temp = null;
            var i = 0;
            l1:
            try
            {
                temp = voipMySqlUnitOfWork.Crm.OrderBy(x => x.id).FirstOrDefault(x => x.contact == customerNumber) ?? null;
            }
            catch
            {
                i++;
                if (i > 3)
                {
                    _logger.Log2("Cannot connect to MySqlServer", Utility.LogType.Warning);
                }
                goto l1;
            }

            if (temp != null)
            {
                if (temp.queue != operatorQueueNumber)
                {
                    voipMySqlUnitOfWork.Crm.Remove(temp);
                    voipMySqlUnitOfWork.Save();
                }
            }
            else
            {
                voipMySqlUnitOfWork.Crm.Add(new crm() { contact = customerNumber, queue = operatorQueueNumber });
                voipMySqlUnitOfWork.SaveAsync(CancellationToken.None).GetAwaiter().GetResult();
            }
        }



        private void Hangup(object sender, HangupEvent e)
        {
            if (e.Cause != 16 && e.Cause != 17)
            {
                return;
            }
            //Cause code: 16
            //Cause description: Normal Clearing

            // Cause code: 17
            // Cause description: User busy

            string operatorQueueNumber;
            string customerNumber;

            if (e.CallerIdNum.Length > 4) // if more than 4 => outgoing else => incoming
            {
                customerNumber = Utility.SimplifiyPhoneNumber(e.CallerIdNum); // dest
                operatorQueueNumber = $"1{Utility.SimplifiyPhoneNumber(e.ConnectedLineName)}"; // queue
            }
            else
            {
                operatorQueueNumber = $"1{Utility.SimplifiyPhoneNumber(e.CallerIdNum)}"; // queue
                customerNumber = Utility.SimplifiyPhoneNumber(e.Connectedlinenum); // dest
            }


            if (customerNumber.Length <= 4)
            {
                return;
            }

            var voipMySqlUnitOfWork = GetVoipMySqlUnitOfWork();


            var uniqueId = voipMySqlUnitOfWork.Cel.FirstOrDefault(x => x.uniqueid == e.UniqueId)
                ?.linkedid;


            var callInformation = new SignalCommunicationInformationModel(Guid.NewGuid().ToString())
            {
                UniqueNumber = uniqueId,
                CustomerConnectedNumber = customerNumber,
                OperatorQueueNumber = operatorQueueNumber,
                StatusTitle = "Hangup"
            };

            var message = JsonConvert.SerializeObject(callInformation);

            _messageBrokerHubContext.Clients.All.SendAsync(operatorQueueNumber,
                    message)
                .GetAwaiter()
                .GetResult();

            _logger.Log2(message, Utility.LogType.Information);
        }


        private Person IsPersonExist(string phoneNumber, out bool isNew)
        {
            var repository = GetRepository();
            var personService = GetPersonService();

            personService.Repository = repository;



            var person = personService.GetAll().FirstOrDefault(x =>
                x.Phone1.Contains(phoneNumber) ||
                x.Phone2.Contains(phoneNumber) ||
                x.Phone3.Contains(phoneNumber));

            if (person == null)
            {
                isNew = true;
                return null;
            }
            else
            {
                isNew = false;
                return person;
            }
        }

        private Customer IsCustomerExist(string phoneNumber, out bool isNew)
        {
            var repository = GetRepository();
            var personService = GetPersonService();
            var customerService = GetCustomerService();
            var caseService = GetCaseService();

            caseService.Repository = repository;
            personService.Repository = repository;
            customerService.Repository = repository;
            caseService.Repository = repository;


            var customer = customerService.GetAll().FirstOrDefault(x =>
                x.Person.Phone1.Contains(phoneNumber) ||
                x.Person.Phone2.Contains(phoneNumber) ||
                x.Person.Phone3.Contains(phoneNumber));

            if (customer == null)
            {
                isNew = true;
                return null;
            }
            else
            {
                isNew = false;
                return customer;
            }
        }

        private Case GetOrCreateCase(string phoneNumber, out bool isNew)
        {
            var repository = GetRepository();
            var personService = GetPersonService();
            var customerService = GetCustomerService();
            var caseService = GetCaseService();

            caseService.Repository = repository;
            personService.Repository = repository;
            customerService.Repository = repository;
            caseService.Repository = repository;


            var customer = IsCustomerExist(phoneNumber, out bool newCustomer);

            if (customer == null)
            {
                var insertingCustomer = new Customer() { };
                var person = IsPersonExist(phoneNumber, out bool newPerson);
                if (person is null)
                {
                    var insertedPerson = personService.Add(new Person() { Phone1 = phoneNumber }).GetAwaiter().GetResult();
                    insertingCustomer.Person = insertedPerson.Entity;
                }
                else
                {
                    insertingCustomer.PersonId = person.Id;
                }

                customer = customerService.AddWithOutCase(insertingCustomer).GetAwaiter().GetResult().Entity;


                isNew = true;
            }
            else
            {
                isNew = false;
            }

            var casee = caseService.GetAll().FirstOrDefault(x => x.CustomerId == customer.Id && x.IsOpen);
            if (casee == null)
            {
                casee = caseService.Add(new Case()
                {
                    CustomerId = customer.Id,
                    StatusBaseId = _taskResultsAndTypesSingleton.Follow
                }).GetAwaiter().GetResult().Entity;

                repository.SaveChanges();
            }

            casee = caseService.FindById(casee.Id)
                .Include(x => x.Customer)
                .Include(x => x.Customer.Person)
                .Include(x => x.Consultant)
                .ThenInclude(x => x.Operator).FirstOrDefault();

            return casee;
        }





        private async System.Threading.Tasks.Task CheckQuitedQueue(CancellationToken cancellationToken)
        {
            new System.Threading.Tasks.Task(() =>
           {
               while (true)
               {
                   var voipMySqlUnitOfWork = GetVoipMySqlUnitOfWork();
                   var repository = GetRepository();
                   var smsService = GetSmsService();
                   var communicationService = GetCommunicationService();
                   var taskService = GetTaskService();
                   var employeeService = GetEmployeeService();

                   smsService.Repository = repository;
                   communicationService.Repository = repository;
                   taskService.Repository = repository;
                   employeeService.Repository = repository;

                   var inboxes = voipMySqlUnitOfWork.QuitedQueues.ToList();

                   if (inboxes is { Count: > 0 })
                   {
                       foreach (var inbox in inboxes)
                       {
                           var cdr = voipMySqlUnitOfWork.Cdr.FirstOrDefault(x => x.uniqueid == inbox.uniqueid);
                           var customerNumber = Utility.SimplifiyPhoneNumber(cdr.src);
                           var casee = GetOrCreateCase(customerNumber, out var isNewCustomer);

                           Employee employee = null;
                           if (cdr.peygiri == "1000")
                           {
                               employee = employeeService.GetAll().Where(x => x.UnitPosition.Position.UniqueName == "consultant").
                                   OrderBy(x => Guid.NewGuid())
                                   .Include(x => x.Person)
                                   .FirstOrDefault();
                           }
                           else
                           {
                               employee = employeeService.GetAll().Include(x => x.Person)
                                   .FirstOrDefault(x => x.Operator.QueueNumber == cdr.peygiri
                                                                  && x.Operator.IsActive == true)
                                   ;
                           }

                           if (employee is null)
                           {
                               throw new Exception("Employee not found");
                           }


                           var communication = communicationService.Add(new Communication()
                           {
                               CustomerId = casee.CustomerId,
                               Employee = employee,
                               TypeBaseId = string.IsNullOrEmpty(cdr.voicemail)
                                   ? _taskResultsAndTypesSingleton.IncomingCall /*تماس*/
                                   : _taskResultsAndTypesSingleton.VoiceMail /*پیغام صوتی*/
                           }).GetAwaiter().GetResult();

                           taskService.Add(new Task()
                           {
                               Employee = employee,
                               CustomerId = casee.CustomerId,
                               Communication = communication.Entity,
                               DuoAt = DateTime.Now,
                               Status = 1,
                               TypeBaseId = _taskResultsAndTypesSingleton.Follow
                           });

                           if (repository.SaveChanges() > 0)
                           {
                               if (isNewCustomer)
                               {
                                   smsService.SendQuickMessage(customerNumber,
                                       Service.Services.SmsService.SmsService.QuickMessageType.OperatorAssignment,
                                       new SmsService.OperatorAssignmentQuickMessage()
                                       {
                                           OperatorFullName = employee.Person.FirstName + " " + employee.Person.LastName
                                       });
                               }

                               CheckAndChangeCustomerAndOperatorLink(customerNumber, employee.Operator.QueueNumber);
                           }
                       }
                   }
                   Thread.Sleep(TimeSpan.FromMinutes(10));
               }
           }).Start();
        }

        #region services
        private IVoipMySqlUnitOfWork GetVoipMySqlUnitOfWork()
        {
            var scope = _serviceProvider.CreateScope();
            return scope.ServiceProvider.GetRequiredService<IVoipMySqlUnitOfWork>();
        }

        private IRedisDataProvider GetRedisDataProvider()
        {
            var scope = _serviceProvider.CreateScope();
            return scope.ServiceProvider.GetRequiredService<IRedisDataProvider>();
        }

        private ISmsService GetSmsService()
        {
            var scope = _serviceProvider.CreateScope();
            return scope.ServiceProvider.GetRequiredService<ISmsService>();
        }

        private IEmployeeService GetEmployeeService()
        {
            var scope = _serviceProvider.CreateScope();
            return scope.ServiceProvider.GetRequiredService<IEmployeeService>();
        }

        private IConfigurationService GetConfigurationService()
        {
            var scope = _serviceProvider.CreateScope();
            return scope.ServiceProvider.GetRequiredService<IConfigurationService>();
        }

        private IPersonService GetPersonService()
        {
            var scope = _serviceProvider.CreateScope();
            return scope.ServiceProvider.GetRequiredService<IPersonService>();
        }

        private ICustomerService GetCustomerService()
        {
            var scope = _serviceProvider.CreateScope();
            return scope.ServiceProvider.GetRequiredService<ICustomerService>();
        }

        private ICaseService GetCaseService()
        {
            var scope = _serviceProvider.CreateScope();
            return scope.ServiceProvider.GetRequiredService<ICaseService>();
        }


        private IAdvertisementService GetAdvertisementService()
        {
            var scope = _serviceProvider.CreateScope();
            return scope.ServiceProvider.GetRequiredService<IAdvertisementService>();
        }

        private ICommunicationService GetCommunicationService()
        {
            var scope = _serviceProvider.CreateScope();
            return scope.ServiceProvider.GetRequiredService<ICommunicationService>();
        }

        private ITaskService GetTaskService()
        {
            var scope = _serviceProvider.CreateScope();
            return scope.ServiceProvider.GetRequiredService<ITaskService>();
        }


        private IRepository GetRepository()
        {
            var scope = _serviceProvider.CreateScope();
            return scope.ServiceProvider.GetRequiredService<IRepository>();
        }

        #endregion

    }
}