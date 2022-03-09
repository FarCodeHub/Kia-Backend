using AutoMapper;
using Infrastructure.Mappings;

namespace Application.CommandQueries.User.Model
{
    public class MenueItemModel : IMapFrom<Domain.Entities.MenuItem>
    {
        public int Id { get; set; }

        public int? PermissionId { get; set; }
        public int? ParentId { get; set; }
        public string? Title { get; set; }
        public string? Icon { get; set; }
        public string? Link { get; set; }
        public string? SubTitle { get; set; }
        public bool? Active { get; set; }
        public bool? Disable { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.MenuItem, MenueItemModel>().IgnoreAllNonExisting();
        }
    }
}
