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

namespace Service.Services.Position
{
    public class PositionService : IPositionService
    {
        private readonly IRepository _repository;

        public PositionService(IRepository repository)
        {
            _repository = repository;
        }

        public IQueryable<Domain.Entities.Position> GetAll()
        {
            return _repository.GetAll<Domain.Entities.Position>();
        }


        public IQueryable<Domain.Entities.Position> FindById(int id)
        {
            return _repository.Find<Domain.Entities.Position>(cfg => cfg.ObjectId(id));
        }

        public async Task<EntityEntry<Domain.Entities.Position>> Add(Domain.Entities.Position inpute)
        {
            return  _repository.Insert(inpute);
        }

        public async Task<EntityEntry<Domain.Entities.Position>> Update(Domain.Entities.Position inpute,
            CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Domain.Entities.Position>(c =>
                    c.ObjectId(inpute.Id))
                .FirstOrDefaultAsync(cancellationToken);

            entity.Title = inpute.Title;
            entity.UniqueName = inpute.UniqueName;
            return _repository.Update(entity);
        }


        public async Task<EntityEntry<Domain.Entities.Position>> SoftDelete(int id, CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Domain.Entities.Position>(c =>
                    c.ObjectId(id))
                .FirstOrDefaultAsync(cancellationToken);

            return _repository.Delete(entity);
        }

        public IQueryable<Domain.Entities.Position> Search(Pagination pagination, List<SearchQueryItem> queries)
        {
            return _repository
                .GetAll<Domain.Entities.Position>(config => config
                    .ConditionExpression(SearchQueryMaker.MakeSearchQuery<Domain.Entities.Position>(queries))
                    .Paginate(pagination));
        }
    }
}