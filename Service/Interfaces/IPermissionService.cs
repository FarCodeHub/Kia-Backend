using Infrastructure.Interfaces;

namespace Service.Interfaces
{
    public interface IPermissionService : ICrudService<Domain.Entities.Permission>, ISearchService<Domain.Entities.Permission>
    {

    }
}