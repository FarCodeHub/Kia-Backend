using Infrastructure.Interfaces;

namespace Service.Interfaces
{
    public interface IPositionService : ICrudService<Domain.Entities.Position>, ISearchService<Domain.Entities.Position>
    {
    }
}