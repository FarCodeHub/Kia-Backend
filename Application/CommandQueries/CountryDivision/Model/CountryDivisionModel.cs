using AutoMapper;
using Infrastructure.Mappings;

namespace Application.CommandQueries.CountryDivision.Model
{
    public class CountryDivisionModel : IMapFrom<Domain.Entities.CountryDivision>
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string ParentTitle { get; set; }
        public string Title { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.CountryDivision, CountryDivisionModel>()
                .ForMember(x=>x.ParentTitle,opt=>opt.MapFrom(x=>x.Parent.Title ?? ""))
                ;
        }
    }
}
