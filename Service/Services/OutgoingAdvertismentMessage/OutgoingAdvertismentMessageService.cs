using Infrastructure.Models;
using Infrastructure.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Service.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Interfaces;

namespace Service.Services.OutgoingAdvertismentMessage
{
    public class OutgoingAdvertismentMessageService : IOutgoingAdvertismentMessageService
    {
        private readonly IRepository _repository;

        public OutgoingAdvertismentMessageService(IRepository repository)
        {
            _repository = repository;
        }

        public IQueryable<Domain.Entities.OutgoingAdvertismentMessage> GetAll()
        {
            return _repository.GetAll<Domain.Entities.OutgoingAdvertismentMessage>();
        }

        public IQueryable<Domain.Entities.OutgoingAdvertismentMessage> FindById(int id)
        {
            return _repository.Find<Domain.Entities.OutgoingAdvertismentMessage>(cfg => cfg.ObjectId(id));
        }

        public async Task<EntityEntry<Domain.Entities.OutgoingAdvertismentMessage>> Add(
            Domain.Entities.OutgoingAdvertismentMessage inpute)
        {
            return  _repository.Insert(inpute);
        }

        public async Task<EntityEntry<Domain.Entities.OutgoingAdvertismentMessage>> Update(Domain.Entities.OutgoingAdvertismentMessage inpute,
            CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Domain.Entities.OutgoingAdvertismentMessage>(c =>
                    c.ObjectId(inpute.Id))
                .FirstOrDefaultAsync(cancellationToken);

            entity.AdvertismentSourceId = inpute.AdvertismentSourceId;
            entity.Reciver = inpute.Reciver;

            return _repository.Update(entity);
        }


        public async Task<EntityEntry<Domain.Entities.OutgoingAdvertismentMessage>> SoftDelete(int id, CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Domain.Entities.OutgoingAdvertismentMessage>(c =>
                    c.ObjectId(id))
                .FirstOrDefaultAsync(cancellationToken);

            return _repository.Delete(entity);
        }

        public IQueryable<Domain.Entities.OutgoingAdvertismentMessage> Search(Pagination pagination, List<SearchQueryItem> queries)
        {
            return _repository
                .GetAll<Domain.Entities.OutgoingAdvertismentMessage>(config => config
                    .ConditionExpression(SearchQueryMaker.MakeSearchQuery<Domain.Entities.OutgoingAdvertismentMessage>(queries))
                    .Paginate(pagination));
        }
    }
}