using AutoMapper;
using Infrastructure.Mappings;

namespace Application.CommandQueries.UnitPosition.Model
{
    public class UnitPositionModel : IMapFrom<Domain.Entities.UnitPosition>
    {
        public int Id { get; set; }

        public int UnitId { get; set; } = default!;
        public int PositionId { get; set; } = default!;
        public string UnitTitle { get; set; }
        public string PositionTitle { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.UnitPosition, UnitPositionModel>()
                .ForMember(x=>x.UnitTitle,opt=>
                    opt.MapFrom(x=>x.Unit.Title))
                .ForMember(x => x.PositionTitle, opt =>
                    opt.MapFrom(x => x.Position.Title))
                ;
        }
    }
}
