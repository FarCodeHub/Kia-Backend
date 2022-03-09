using System.Linq;
using Infrastructure.Interfaces;

namespace Service.Interfaces
{
    public interface IBaseValueService : ICrudService<Domain.Entities.BaseValue>, ISearchService<Domain.Entities.BaseValue>
    {
        public IQueryable<Domain.Entities.BaseValue> GetAllByBaseValueTypeUniqueName(string uniquename);

        public IQueryable<Domain.Entities.BaseValue> GetByBaseValueUniqueName(string uniquename);
        public IQueryable<Domain.Entities.BaseValue> GetAllByUniqueName(string uniqueName);

        public IQueryable<Domain.Entities.BaseValue> GetAllByBaseValueTypeId(int baseValuetypeId);

    }
}