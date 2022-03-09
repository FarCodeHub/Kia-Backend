using Infrastructure.Interfaces;

namespace Service.Interfaces
{
    public interface IBaseValueTypeService : ICrudService<Domain.Entities.BaseValueType>, ISearchService<Domain.Entities.BaseValueType>
    {

    }
}