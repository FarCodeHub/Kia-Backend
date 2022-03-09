using AutoMapper;
using Infrastructure.Mappings;

namespace Application.CommandQueries.Sms.Model
{
    public class PersonPhonesModel : IMapFrom<Domain.Entities.Person>
    {
        public int PersonId { get; set; }
        public string PhoneNumber { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.Person, PersonPhonesModel>()
                .ForMember(x => x.PersonId, opt => opt.MapFrom(x => x.Id))
                .ForMember(x => x.PhoneNumber, opt => opt.MapFrom(x => x.Phone1 ?? x.Phone2 ?? x.Phone3));
        }
    }
}
