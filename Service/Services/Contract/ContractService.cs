using Infrastructure.Models;
using Infrastructure.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Service.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using Infrastructure.Exceptions;
using Infrastructure.Interfaces;
using Service.Services.BaseValue;

namespace Service.Services.Contract
{
    public class ContractService : IContractService
    {
        private readonly IRepository _repository;
        private readonly ITaskService _taskService;
        private readonly IUpLoader _upLoader;
        private readonly ICommissionService _commissionService;
        private readonly ICaseService _caseService;
        private readonly TaskResultsAndTypesSingleton _taskResultsAndTypesSingleton;
        private readonly IHandledErrorManager _handledErrorManager;
        public ContractService(IRepository repository, ITaskService taskService, IUpLoader upLoader, ICommissionService commissionService, ICaseService caseService, IHandledErrorManager handledErrorManager)
        {
            _repository = repository;
            _taskService = taskService;
            _upLoader = upLoader;
            _commissionService = commissionService;
            _caseService = caseService;
            _handledErrorManager = handledErrorManager;
            _taskResultsAndTypesSingleton = TaskResultsAndTypesSingleton.Instance(_repository);
        }

        public IQueryable<Domain.Entities.Contract> GetAll()
        {
            return _repository.GetAll<Domain.Entities.Contract>();
        }

        public IQueryable<Domain.Entities.Contract> FindById(int id)
        {
            return _repository.Find<Domain.Entities.Contract>(cfg => cfg.ObjectId(id));
        }


        public async Task<IQueryable<Domain.Entities.Task>> GetAllInvolved(int cutomerId, int contractTaskId)
        {
            var query = _repository.GetQuery<Domain.Entities.Task>()
                .OrderBy(x => x.CreatedAt)
                .Where(x =>
                    x.CustomerId == cutomerId && x.Id != contractTaskId &&
                    x.ResultBaseId != 0 &&
                    x.ResultBaseId != 1);

            var allTasks = await query.ToListAsync();


            if (allTasks.Count(x => x.ResultBaseId == 100) > 0)
            {
                var previousContractTask = await query.LastOrDefaultAsync(x => x.ResultBaseId == 100);

                return query.Where(x => x.CreatedAt > previousContractTask.CreatedAt);
            }

            return query;
        }

        public async Task<EntityEntry<Domain.Entities.Contract>> Add(Domain.Entities.Contract inpute)
        {
            return _repository.Insert(inpute);
        }

        public async Task<EntityEntry<Domain.Entities.Contract>> Update(Domain.Entities.Contract inpute, CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Domain.Entities.Contract>(c =>
                    c.ObjectId(inpute.Id))
                .FirstOrDefaultAsync(cancellationToken);

            entity.Descriptions = inpute.Descriptions;
 
            return _repository.Update(entity);

        }


        public async Task<EntityEntry<Domain.Entities.Contract>> Add(Domain.Entities.Contract inpute, string[] requestAttachmentsUrl)
        {
            var contract = await Add(inpute);
            foreach (var s in requestAttachmentsUrl)
            {
                var fileRes = await _upLoader.UpLoadAsync(s, CustomPath.ContractAttachment);
                _repository.Insert(new ContractAttachment()
                { Contract = contract.Entity, FilePath = fileRes.ReletivePath });
            }
            return contract;
        }

        public async Task<EntityEntry<Domain.Entities.Contract>> Update(Domain.Entities.Contract inpute,
            ICollection<int> involvedEployeesId,
            string[] requestAttachmentsUrl,
            CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Domain.Entities.Contract>(c =>
                    c.ObjectId(inpute.Id))
                .FirstOrDefaultAsync(cancellationToken);

            entity.Descriptions = inpute.Descriptions;

            foreach (var removedInvolved in entity.Commissions.Select(x => x.EmployeeId).Except(involvedEployeesId))
            {
                var deletingEntity = await _commissionService.GetAll()
                    .FirstOrDefaultAsync(x => x.ContractId == entity.Id && x.EmployeeId == removedInvolved, CancellationToken.None);

                await _commissionService.SoftDelete(deletingEntity.Id, cancellationToken);
            }

            foreach (var addedInvolved in involvedEployeesId.Except(entity.Commissions.Select(x => x.EmployeeId)))
            {
                await _commissionService.Add(new Domain.Entities.Commission()
                { EmployeeId = addedInvolved, Contract = entity, IsPaid = false, Amount = 0 });
            }


            foreach (var removedAttachment in entity.ContractAttachments.Select(x => x.FilePath).Except(requestAttachmentsUrl))
            {
                await _upLoader.DeleteAsync(removedAttachment, CustomPath.ContractAttachment,
                    CancellationToken.None);
            }

            foreach (var addedAttachment in requestAttachmentsUrl.Except(entity.ContractAttachments.Select(x => x.FilePath)))
            {
                var fileRes = await _upLoader.UpLoadAsync(addedAttachment, CustomPath.ContractAttachment, cancellationToken);
                _repository.Insert(new ContractAttachment()
                { Contract = entity, FilePath = fileRes.ReletivePath });
            }

            return _repository.Update(entity);
        }

        public async Task<EntityEntry<Domain.Entities.Contract>> Cancle(int id, CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Domain.Entities.Contract>(c =>
                    c.ObjectId(id))
                .FirstOrDefaultAsync(cancellationToken);

            var @case = await _caseService.FindById(entity.CaseId).FirstOrDefaultAsync(cancellationToken);
            @case.StatusBaseId = _taskResultsAndTypesSingleton.CancleContract;
            await _caseService.Update(@case, CancellationToken.None);

            foreach (var removedInvolved in entity.Commissions.Select(x => x.EmployeeId))
            {
                var deletingEntity = await _commissionService.GetAll()
                    .FirstOrDefaultAsync(x => x.ContractId == entity.Id && x.EmployeeId == removedInvolved, CancellationToken.None);

                if (deletingEntity.IsPaid)
                {
                    _handledErrorManager.Throw<DependencyViolation>(new List<string>() { "Commission" });
                }

                await _commissionService.SoftDelete(deletingEntity.Id, cancellationToken);
            }

            foreach (var removedAttachment in entity.ContractAttachments.Select(x => x.FilePath))
            {
                await _upLoader.DeleteAsync(removedAttachment, CustomPath.ContractAttachment,
                    CancellationToken.None);
            }

            return _repository.Delete(entity);
        }


        public async Task<EntityEntry<Domain.Entities.Contract>> SoftDelete(int id, CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Domain.Entities.Contract>(c =>
                    c.ObjectId(id))
                .FirstOrDefaultAsync(cancellationToken);

            return _repository.Delete(entity);
        }

        public IQueryable<Domain.Entities.Contract> Search(Pagination pagination, List<SearchQueryItem> queries)
        {
            return _repository
                .GetAll<Domain.Entities.Contract>(config => config
                    .ConditionExpression(SearchQueryMaker.MakeSearchQuery<Domain.Entities.Contract>(queries))
                    .Paginate(pagination));
        }
    }
}