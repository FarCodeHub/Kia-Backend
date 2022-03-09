using System.Linq;
using AutoMapper;
using Infrastructure.Mappings;

namespace Application.CommandQueries.Sms.Model
{
    public class PersonToSendModel : IMapFrom<Domain.Entities.Person>
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public int? ZipCodeId { get; set; }
        public string? Address { get; set; }
        public string ZipCodePlaceTitle { get; set; }
        public int EmployeeId { get; set; }
        public string? Phone { get; set; }
        public int LastTaskResultId { get; set; }
        public string LastTaskResultTitle { get; set; }
        public string OperatorFullName { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.Person, PersonToSendModel>()
                .ForMember(x => x.LastTaskResultId, opt => opt.MapFrom(x => x.Customer.Tasks.OrderBy(x=>x.Id).LastOrDefault().ResultBaseId))
                .ForMember(x => x.LastTaskResultTitle, opt => opt.MapFrom(x => x.Customer.Tasks.OrderBy(x=>x.Id).LastOrDefault().ResultBase.Title))
                .ForMember(x => x.FullName, opt => opt.MapFrom(x => x.FirstName + " " + x.LastName))
                .ForMember(x => x.EmployeeId, opt => opt.MapFrom(x => x.Customer.Communications.OrderBy(x=>x.Id).LastOrDefault().EmployeeId))
                .ForMember(x => x.OperatorFullName, opt =>
                    opt.MapFrom(x => x.Customer.Cases.OrderBy(t=>t.Id).LastOrDefault(o=>o.IsOpen).Consultant.Person.FirstName + " " 
                        + x.Customer.Cases.OrderBy(t => t.Id).LastOrDefault(o=>o.IsOpen).Consultant.Person.LastName))
                .ForMember(x=>x.ZipCodePlaceTitle,opt=>opt.MapFrom(x=> $"{x.ZipCode.Parent.Title}-{x.ZipCode.Title}"))
                .ForMember(x => x.Phone, opt => opt.MapFrom(x => x.Phone1 ?? x.Phone2 ?? x.Phone3));
        }
    }
}
