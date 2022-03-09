using Infrastructure.Interfaces;

namespace Service.Interfaces
{
    public interface IOperatorService : ICrudService<Domain.Entities.Operator>, ISearchService<Domain.Entities.Operator>
    {

    }
}