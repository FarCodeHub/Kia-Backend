using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Infrastructure.Mappings;

namespace Application.CommandQueries.Role.Model
{
    public class RoleModel : IMapFrom<Domain.Entities.Role>
    {
        public int Id { get; set; }
        public string LevelCode { get; set; }
        public string Title { get; set; }
        public string UniqueName { get; set; }
        public string Description { get; set; }
        public int? ParentId { get; set; }
        public string ParentTitle { get; set; }
        public IList<int> PermissionsIdList { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.Role, RoleModel>().IgnoreAllNonExisting()
                .ForMember(x => x.PermissionsIdList, opt => opt.MapFrom(
                    x => x.RolePermissionRoles.Select(x => x.PermissionId)))
                .ForMember(x=>x.ParentTitle,opt=>opt.MapFrom(x=>x.Parent.Title));
        }
    }
}
