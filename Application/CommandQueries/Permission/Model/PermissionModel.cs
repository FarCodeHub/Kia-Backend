using AutoMapper;
using Infrastructure.Mappings;

namespace Application.CommandQueries.Permission.Model
{
    public class PermissionModel : IMapFrom<Domain.Entities.Permission>
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Title { get; set; }
        public string UniqueName { get; set; }
        public string LevelCode { get; set; }

        public string ParentTitle { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.Permission, PermissionModel>()
                .ForMember(x=>x.ParentTitle,opt=>opt.MapFrom(x=>x.Parent.Title))
                ;
        }
    }
}
