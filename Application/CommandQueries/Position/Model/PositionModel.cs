using AutoMapper;
using Infrastructure.Mappings;

namespace Application.CommandQueries.Position.Model
{
    public class PositionModel : IMapFrom<Domain.Entities.Position>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string UniqueName { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.Position, PositionModel>();
        }
    }
}
