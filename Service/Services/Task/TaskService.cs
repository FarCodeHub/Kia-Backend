using System;
using Infrastructure.Models;
using Infrastructure.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Service.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.CurrentUserAccessor;
using Infrastructure.Exceptions;
using Infrastructure.Interfaces;
using Service.Services.BaseValue;

namespace Service.Services.Task
{
    public class TaskService : ITaskService
    {
        private  IRepository _repository;
        private readonly ISessionSurveyService _sessionSurveyService;
        private readonly IEmployeeService _employeeService;
        private readonly TaskResultsAndTypesSingleton _taskResultsAndTypesSingletoonService;
        private readonly ICaseService _caseService;
        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly ICustomerService _customerService;
        private readonly IHandledErrorManager _handledErrorManager;
        public TaskService(IRepository repository, ISessionSurveyService sessionSurveyService, IEmployeeService employeeService, ICaseService caseService, ICurrentUserAccessor currentUserAccessor, ICustomerService customerService, IHandledErrorManager handledErrorManager)
        {
            _repository = repository;
            _sessionSurveyService = sessionSurveyService;
            _employeeService = employeeService;
            _caseService = caseService;
            _currentUserAccessor = currentUserAccessor;
            _customerService = customerService;
            _handledErrorManager = handledErrorManager;
            _taskResultsAndTypesSingletoonService = TaskResultsAndTypesSingleton.Instance(_repository);
        }

        public IRepository Repository { get => _repository; set => _repository = value; }

        public IQueryable<Domain.Entities.Task> GetAll()
        {
            return _repository.GetAll<Domain.Entities.Task>();
        }

        public IQueryable<Domain.Entities.Task> FindById(int id)
        {
            return _repository.Find<Domain.Entities.Task>(cfg => cfg.ObjectId(id));
        }

        public async Task<Domain.Entities.Task> StartTask(int taskId)
        {
            var task = await FindById(taskId).FirstOrDefaultAsync();

            if (task.StartAt != null)
            {
                _handledErrorManager.Throw<TaskIsStartedAlredy>();
            }

            task.StartAt = DateTime.Now;
            task.Status = 2;
            if (task.ResultBaseId == _taskResultsAndTypesSingletoonService.ApplyToVisitTheLand ||
                     task.ResultBaseId == _taskResultsAndTypesSingletoonService.Appointment ||
                     task.ResultBaseId == _taskResultsAndTypesSingletoonService.Reference)
            {
                var casee = await _caseService.FindById(task.CaseId).FirstOrDefaultAsync();
                casee.PresentorId = task.EmployeeId;
                casee.StatusBaseId = task.TypeBaseId;
                await _caseService.Update(casee, CancellationToken.None);
            }
            var updatedTask = await Update(task, CancellationToken.None);
            return updatedTask.Entity;
        }

        public async Task<EntityEntry<Domain.Entities.Task>> EndTask(int taskId, int communicationId, int resultBaseId, int? assignToEmployee, DateTime nextTaskDueAt,string descriptions)
        {
            var task = await FindById(taskId)
                .Include(x => x.Employee)
                .FirstOrDefaultAsync();

            if (task.ResultBaseId != null && task.EndAt != null)
            {
                _handledErrorManager.Throw<TaskIsEndAlredy>();
            }
            if (task.StartAt == null)
            {
                _handledErrorManager.Throw<TasHasNotStartedYet>();
            }


            task.EndAt = DateTime.Now;
            task.Status = 3;
            task.Descriptions = descriptions;
            task.ResultBaseId = resultBaseId;
            var updatedTask = await Update(task, CancellationToken.None);
            var casee = await _caseService.FindById(task.CaseId).FirstOrDefaultAsync();
            casee.StatusBaseId = resultBaseId;
            await _caseService.Update(casee, CancellationToken.None);

            var newTask = new Domain.Entities.Task()
            {
                CustomerId = task.CustomerId,
                CommunicationId = communicationId,
                DuoAt = nextTaskDueAt,
                ParentId = taskId,
                Status = 1,
                CaseId = task.CaseId
            };

            if (resultBaseId == _taskResultsAndTypesSingletoonService.RequestApplyToVisitTheLand
                || resultBaseId == _taskResultsAndTypesSingletoonService.RequestAppointment)
            {
                newTask.StartAt = DateTime.Now;
                newTask.Employee = await (from e in _employeeService.GetAll()
                                          where e.UnitPosition.Position.UniqueName == "presenterSupervisor" &&
                                                e.UnitPosition.UnitId == _currentUserAccessor.GetUnitId()
                                          select e).FirstOrDefaultAsync();
                if (newTask.Employee == null)
                {
                    _handledErrorManager.Throw<SpecificEmployeeNotFound>();
                }
                if (resultBaseId == _taskResultsAndTypesSingletoonService.RequestApplyToVisitTheLand)
                    newTask.TypeBaseId = _taskResultsAndTypesSingletoonService.ConfirmApplyToVisitTheLand;
                if (resultBaseId == _taskResultsAndTypesSingletoonService.RequestAppointment)
                    newTask.TypeBaseId = _taskResultsAndTypesSingletoonService.ConfirmAppointment;
            }
            else if (resultBaseId == _taskResultsAndTypesSingletoonService.RequestCancel ||
                     resultBaseId == _taskResultsAndTypesSingletoonService.RequestReference)
            {
                newTask.StartAt = DateTime.Now;
                newTask.Employee = await _employeeService.GetAll()
                    .FirstOrDefaultAsync(x => x.UnitPosition.UnitId == _currentUserAccessor.GetUnitId() &&
                                              x.UnitPosition.Position.UniqueName == "consultorSupervisor"
                    );
                if (newTask.Employee == null)
                {
                    _handledErrorManager.Throw<SpecificEmployeeNotFound>();
                }
            }
            else if (resultBaseId == _taskResultsAndTypesSingletoonService.ApplyToVisitTheLand ||
                     resultBaseId == _taskResultsAndTypesSingletoonService.Appointment ||
                     resultBaseId == _taskResultsAndTypesSingletoonService.Reference)
            {
                newTask.StartAt = DateTime.Now;
                casee.PresentorId = assignToEmployee;
                await _caseService.Update(casee, CancellationToken.None);
                newTask.Employee = await _employeeService.FindById(assignToEmployee ?? throw new Exception("Assigned employee not found")).FirstOrDefaultAsync();
                return updatedTask;
            }
            else if (resultBaseId == _taskResultsAndTypesSingletoonService.Cancel ||
                    resultBaseId == _taskResultsAndTypesSingletoonService.ApplyToContract ||
                    resultBaseId == _taskResultsAndTypesSingletoonService.SendToBlackList)
            {
                await _caseService.Close(task.CaseId, _taskResultsAndTypesSingletoonService.SendToBlackList, CancellationToken.None);
                if (resultBaseId == _taskResultsAndTypesSingletoonService.SendToBlackList)
                {
                    await _customerService.Block(task.CustomerId, CancellationToken.None);
                }

                return updatedTask;
            }
            else
            {
                newTask.StartAt = DateTime.Now;
                newTask.TypeBaseId = resultBaseId;
                newTask.Employee = task.Employee;
            }

            newTask.EmployeeId = newTask.Employee.Id;

            await Add(newTask);

            return updatedTask;
        }

        public async Task<EntityEntry<Domain.Entities.Task>> Add(Domain.Entities.Task inpute)
        {
            var casee = await _caseService.GetAll().AsNoTracking().FirstOrDefaultAsync(x => x.CustomerId == inpute.CustomerId && x.IsOpen);
            inpute.CaseId = casee.Id;
            inpute.Status = 1;
            var task = _repository.Insert(inpute);
            return task;
        }


        public async Task<EntityEntry<Domain.Entities.Task>> Add(Domain.Entities.Task inpute, IList<Domain.Entities.SessionSurvey> sessionSurveys)
        {
            var task = await Add(inpute);
            foreach (var sessionSurvey in sessionSurveys)
            {
                sessionSurvey.Task = task.Entity;
                await _sessionSurveyService.Add(sessionSurvey);
            }

            return task;
        }

        public async Task<EntityEntry<Domain.Entities.Task>> Update(Domain.Entities.Task inpute,
            CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Domain.Entities.Task>(c =>
                    c.ObjectId(inpute.Id))
                .FirstOrDefaultAsync(cancellationToken);

            entity.StartAt = inpute.StartAt;
            entity.CustomerId = inpute.CustomerId;
            entity.EndAt = inpute.EndAt;
            entity.EmployeeId = inpute.EmployeeId;
            entity.ParentId = inpute.ParentId;
            entity.ProjectId = inpute.ProjectId;
            entity.Status = inpute.Status;
            entity.ResultBaseId = inpute.ResultBaseId;
            entity.Descriptions = inpute.Descriptions;
            return _repository.Update(entity);
        }


        public async Task<EntityEntry<Domain.Entities.Task>> SoftDelete(int id, CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Domain.Entities.Task>(c =>
                    c.ObjectId(id))
                .FirstOrDefaultAsync(cancellationToken);

            return _repository.Delete(entity);
        }

        public IQueryable<Domain.Entities.Task> Search(Pagination pagination, List<SearchQueryItem> queries)
        {
            return _repository
                .GetAll<Domain.Entities.Task>(config => config
                    .ConditionExpression(SearchQueryMaker.MakeSearchQuery<Domain.Entities.Task>(queries))
                    .Paginate(pagination));
        }
    }
}