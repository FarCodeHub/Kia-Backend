using System;
using System.Linq;
using AutoMapper;
using Infrastructure.Mappings;

namespace Application.CommandQueries.Case.Model
{
    public class CaseModel : IMapFrom<Domain.Entities.Case>
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int? ConsultantId { get; set; }
        public int? PresentorId { get; set; }
        public int StatusBaseId { get; set; }
        public int? ProjectId { get; set; }
        public string ConsultantFullName { get; set; }
        public string CustomerFullName { get; set; }
        public string CustomerPhone { get; set; }
        public string PresentorFullName { get; set; }
        public string StatusBaseTitle { get; set; }
        public string ProjectTitle { get; set; }
        public DateTime CreatedAt { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.Case, CaseModel>()
                .ForMember(x => x.ProjectId, opt =>
                    opt.MapFrom(x => x.Tasks.OrderBy(t => t.Id).LastOrDefault().ProjectId
                    ))
                .ForMember(x=>x.ConsultantFullName,opt=>opt.MapFrom(x=>x.Consultant.Person.FirstName + " " + x.Consultant.Person.LastName))
                .ForMember(x=>x.CustomerFullName, opt=>opt.MapFrom(x=>x.Customer.Person.FirstName + " " + x.Customer.Person.LastName))
                .ForMember(x=>x.CustomerPhone, opt=>opt.MapFrom(x=>x.Customer.Person.Phone1 ?? x.Customer.Person.Phone2 ?? x.Customer.Person.Phone3))
                .ForMember(x=>x.PresentorFullName,opt=>opt.MapFrom(x=>x.Presentor.Person.FirstName + " " + x.Presentor.Person.LastName))
                .ForMember(x=>x.StatusBaseTitle,opt=>opt.MapFrom(x=>x.StatusBase.Title))
                .ForMember(x=>x.ProjectTitle,opt=> opt.MapFrom(x => x.Tasks.OrderBy(t => t.Id).LastOrDefault().Project.Title))
                ;
        }
    }
}
