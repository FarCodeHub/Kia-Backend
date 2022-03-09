using Application.CommandQueries.Person.Model;
using AutoMapper;
using Infrastructure.Mappings;

namespace Application.CommandQueries.Customer.Model
{
    public class CustomerModel : IMapFrom<Domain.Entities.Customer>
    {
        public int Id { get; set; }
        public int PersonId { get; set; } = default!;
        public PersonModel PersonModel { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.Customer, CustomerModel>()
                .ForMember(x=>x.PersonModel,opt=>opt.MapFrom(x=>x.Person));
        }
    }
}
