using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using Infrastructure.CurrentUserAccessor;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using Infrastructure.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Service.Interfaces;

namespace Service.Services.AdvertismentSource
{
    public class AdvertisementService : IAdvertisementService
    {
        private  IRepository _repository;
        public IRepository Repository { get => _repository; set => _repository = value; }

        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly IBaseValueService _baseValueService;

        public AdvertisementService(IRepository repository, ICurrentUserAccessor currentUserAccessor, IBaseValueService baseValueService)
        {
            _repository = repository;
            _currentUserAccessor = currentUserAccessor;
            _baseValueService = baseValueService;
        }

        public IQueryable<Domain.Entities.Advertisement> GetAll()
        {
            return _repository.GetAll<Domain.Entities.Advertisement>();
        }

        public IQueryable<Domain.Entities.Advertisement> FindById(int id)
        {
            return _repository.Find<Domain.Entities.Advertisement>(cfg => cfg.ObjectId(id));
        }

        public IQueryable<Domain.Entities.Advertisement> FindByFeedBackNumber(int feedbackNumber)
        {
            return _repository.Find<Domain.Entities.Advertisement>(cfg => cfg.ConditionExpression(x=>x.FeedbackNumber == feedbackNumber &&
                x.StartDateTime < DateTime.Now &&
                x.EndDateTime > DateTime.Now));
        }


        public async Task<EntityEntry<Advertisement>> Add(Advertisement inpute)
        {
            return _repository.Insert(inpute);
        }

        public async Task<bool> IsUsableFeedbackNumber(int feedbackNumber, DateTime startDateTime, DateTime endDateTime)
        {
            return !(await _repository.GetQuery<Advertisement>().AnyAsync(x =>
               x.FeedbackNumber == feedbackNumber && 
               x.EndDateTime >= startDateTime));
        }



        public async Task<EntityEntry<Domain.Entities.Advertisement>> Update(Domain.Entities.Advertisement inpute,
            CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Domain.Entities.Advertisement>(c =>
                    c.ObjectId(inpute.Id))
                .FirstOrDefaultAsync(cancellationToken);

            entity.Title = inpute.Title;
            entity.AdvertisementTypeBaseId = inpute.AdvertisementTypeBaseId;
            entity.Reputation = inpute.Reputation;
            entity.HeadLineNumberBaseId = inpute.HeadLineNumberBaseId;
            entity.HostName = inpute.HostName;
            entity.FeedbackNumber = inpute.FeedbackNumber;
            entity.Expense = inpute.Expense;
            entity.StartDateTime = inpute.StartDateTime;
            entity.EndDateTime = inpute.EndDateTime;
            entity.Descriptions = inpute.Descriptions;

            return _repository.Update(entity);
        }


        public async Task<EntityEntry<Domain.Entities.Advertisement>> SoftDelete(int id, CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Domain.Entities.Advertisement>(c =>
                    c.ObjectId(id))
                .FirstOrDefaultAsync(cancellationToken);

            return _repository.Delete(entity);
        }



        public IQueryable<Domain.Entities.Advertisement> Search(Pagination pagination, List<SearchQueryItem> queries)
        {
            return _repository
                .GetAll<Domain.Entities.Advertisement>(config => config
                    .ConditionExpression(SearchQueryMaker.MakeSearchQuery<Domain.Entities.Advertisement>(queries))
                    .Paginate(pagination));
        }
    }
}