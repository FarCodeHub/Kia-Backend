using AutoMapper;
using Infrastructure.Mappings;

namespace Application.CommandQueries.Operator.Model
{
    public class OperatorModel : IMapFrom<Domain.Entities.Operator>
    {
        public int Id { get; set; }

        public string ExtentionNumber { get; set; } = default!;
        public string QueueNumber { get; set; } = default!;
        public int EmployeeId { get; set; } = default!;
        public string FullName { get; set; }
        public int PersonId { get; set; }
        public string UnitPositionTitle { get; set; }
        public string? ProfilePhotoUrl { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.Operator, OperatorModel>()
                .ForMember(x => x.FullName, opt => opt.MapFrom(x => x.Employee.Person.FirstName + " " + x.Employee.Person.LastName))
                .ForMember(x => x.UnitPositionTitle, opt => opt.MapFrom(x => x.Employee.UnitPosition.Unit.Title + "-" + x.Employee.UnitPosition.Position.Title))
                .ForMember(x => x.PersonId, opt => opt.MapFrom(x => x.Employee.PersonId))
                .ForMember(x => x.ProfilePhotoUrl, opt => opt.MapFrom(x => x.Employee.Person.ProfilePhotoUrl))
                ;
        }
    }
}
