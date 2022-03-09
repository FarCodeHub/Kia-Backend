using System;
using AutoMapper;
using Infrastructure.Mappings;

namespace Application.CommandQueries.Task.Model
{
    public class ToDueTaskModel : IMapFrom<Domain.Entities.Task>
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public int? CommunicationId { get; set; }
        public int TypeBaseId { get; set; }
        public int EmployeeId { get; set; }
        public int CustomerId { get; set; }
        public int? ProjectId { get; set; }
        public DateTime? StartAt { get; set; }
        public DateTime DuoAt { get; set; }
        public DateTime? EndAt { get; set; }
        public int Status { get; set; }
        public string StatusTitle { get; set; }
        public int? ResultBaseId { get; set; }
        public string? ProjectTitle { get; set; }
        public DateTime CreatedAt { get; set; }
        public virtual string CustomerFullName { get; set; } = default!;
        public string ExtentionNumber { get; set; }
        public string EmployeeFullName { get; set; }
        public string CustomerPhoneNumber1 { get; set; }
        public string CustomerPhoneNumber2 { get; set; }
        public string CustomerPhoneNumber3 { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.Task, ToDueTaskModel>()
                .ForMember(x => x.CreatedAt, opt => opt.MapFrom(x => x.CreatedAt))
                .ForMember(x => x.ProjectTitle, opt => opt.MapFrom(x => x.Project.Title))
                .ForMember(x => x.CustomerFullName, opt => opt.MapFrom(x => x.Customer.Person.FirstName + " " + x.Customer.Person.LastName))
                .ForMember(x => x.CustomerPhoneNumber1, opt => opt.MapFrom(x => x.Customer.Person.Phone1))
                .ForMember(x => x.CustomerPhoneNumber2, opt => opt.MapFrom(x => x.Customer.Person.Phone2))
                .ForMember(x => x.CustomerPhoneNumber3, opt => opt.MapFrom(x => x.Customer.Person.Phone3))
                .ForMember(x => x.EmployeeFullName, opt => opt.MapFrom(x => x.Employee.Person.FirstName + " " + x.Employee.Person.LastName))
                .ForMember(x => x.ExtentionNumber, opt => opt.MapFrom(x => x.Employee.Operator.ExtentionNumber))
                ;
        }
    }
}
