using AutoMapper;
using Infrastructure.Mappings;

namespace Application.CommandQueries.Branch.Model
{
    public class BranchModel : IMapFrom<Domain.Entities.Branch>
    {
        public int Id { get; set; }
        public string? Address { get; set; }
        public string Title { get; set; }
        public double? Lat { get; set; }
        public double? Lng { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.Branch, BranchModel>()
                .ForMember(x => x.Lat, opt => opt.MapFrom(x => x.Location.PointOnSurface.Y))
                .ForMember(x => x.Lng, opt => opt.MapFrom(x => x.Location.PointOnSurface.X))
                ;
        }
    }
}
