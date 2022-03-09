using AutoMapper;
using Infrastructure.Mappings;

namespace Application.CommandQueries.RolePermission.Model
{
    public class RolePermissionModel : IMapFrom<Domain.Entities.RolePermission>
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public int PermissionId { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.RolePermission, RolePermissionModel>();
        }
    }
}
