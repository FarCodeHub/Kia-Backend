using AutoMapper;
using Infrastructure.Mappings;

namespace Application.CommandQueries.UserRole.Model
{
    public class UserRoleModel : IMapFrom<Domain.Entities.UserRole>
    {
        public int Id { get; set; }
        public int RoleId { get; set; } = default!;
        public int UserId { get; set; } = default!;
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.UserRole, UserRoleModel>().IgnoreAllNonExisting();
        }
    }
}
