using Infrastructure.Interfaces;

namespace Service.Interfaces
{
    public interface IUserRoleService : ICrudService<Domain.Entities.UserRole>, ISearchService<Domain.Entities.UserRole>
    {

    }
}