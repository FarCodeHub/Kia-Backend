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

namespace Service.Services.UnitPosition
{
    public class UnitPositionService : IUnitPositionService
    {
        private readonly IRepository _repository;

        public UnitPositionService(IRepository repository)
        {
            _repository = repository;
        }

        public IQueryable<Domain.Entities.UnitPosition> GetAll()
        {
            return _repository.GetAll<Domain.Entities.UnitPosition>();
        }

     
        public IQueryable<Domain.Entities.UnitPosition> GetByUnitIdAndPositionId(int unitId, int PositionId)
        {
            return _repository.GetAll<Domain.Entities.UnitPosition>(cfg =>
                cfg.ConditionExpression(x => x.UnitId == unitId && x.PositionId == PositionId));
        }
        public IQueryable<Domain.Entities.UnitPosition> FindById(int id)
        {
            return _repository.Find<Domain.Entities.UnitPosition>(cfg => cfg.ObjectId(id));
        }

        public async Task<EntityEntry<Domain.Entities.UnitPosition>> Add(Domain.Entities.UnitPosition inpute)
        {
            return  _repository.Insert(inpute);
        }

        public async Task<EntityEntry<Domain.Entities.UnitPosition>> Update(Domain.Entities.UnitPosition inpute,
            CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Domain.Entities.UnitPosition>(c =>
                    c.ObjectId(inpute.Id))
                .FirstOrDefaultAsync(cancellationToken);

            entity = new DynamicUpdator(_repository.Model)
                .Update<Domain.Entities.UnitPosition>(entity, inpute);

            return _repository.Update(entity);
        }


        public async Task<EntityEntry<Domain.Entities.UnitPosition>> SoftDelete(int id, CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Domain.Entities.UnitPosition>(c =>
                    c.ObjectId(id))
                .FirstOrDefaultAsync(cancellationToken);

            return _repository.Delete(entity);
        }

        public IQueryable<Domain.Entities.UnitPosition> Search(Pagination pagination, List<SearchQueryItem> queries)
        {
            return _repository
                .GetAll<Domain.Entities.UnitPosition>(config => config
                    .ConditionExpression(SearchQueryMaker.MakeSearchQuery<Domain.Entities.UnitPosition>(queries))
                    .Paginate(pagination));
        }

        public IQueryable<Domain.Entities.UnitPosition> GetAllByUnitId(int unitId)
        {
            return _repository.GetAll<Domain.Entities.UnitPosition>(cfg =>
                cfg.ConditionExpression(x=>x.UnitId == unitId));
        }
    }
}