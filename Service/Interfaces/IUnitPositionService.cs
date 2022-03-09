using System.Linq;
using Domain.Entities;
using Infrastructure.Interfaces;

namespace Service.Interfaces
{
    public interface IUnitPositionService : ICrudService<Domain.Entities.UnitPosition>, ISearchService<Domain.Entities.UnitPosition>
    {
        IQueryable<UnitPosition> GetAllByUnitId(int unitId);
        public IQueryable<Domain.Entities.UnitPosition> GetByUnitIdAndPositionId(int unitId, int PositionId);


    }
}