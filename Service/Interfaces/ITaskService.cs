using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Task = Domain.Entities.Task;

namespace Service.Interfaces
{
    public interface ITaskService : ICrudService<Domain.Entities.Task>
    {
        IRepository Repository { get; set; }

        Task<EntityEntry<Domain.Entities.Task>> Add(Domain.Entities.Task inpute,
            IList<Domain.Entities.SessionSurvey> sessionSurveys);

        Task<EntityEntry<Domain.Entities.Task>> EndTask(int taskId, int communicationId, int resultBaseId, int? assignToEmployee, DateTime dueAt,string descriptions);

         Task<Task> StartTask(int requestTaskId);


    }
}