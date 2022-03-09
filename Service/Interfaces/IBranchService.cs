using Infrastructure.Interfaces;

namespace Service.Interfaces
{
    public interface IBranchService : ICrudService<Domain.Entities.Branch>, ISearchService<Domain.Entities.Branch>
    {

    }
}