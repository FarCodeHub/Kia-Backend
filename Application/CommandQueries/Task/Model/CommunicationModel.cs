using System;
using System.Linq;
using AutoMapper;
using Infrastructure.Mappings;

namespace Application.CommandQueries.Task.Model
{
    public class CommunicationModel : IMapFrom<Domain.Entities.Communication>
    {
        public int Id { get; set; }
        public int? TypeBaseId { get; set; }
        public string TypeBaseTitle { get; set; }
        public string TypeBaseUniqueName { get; set; }
        public int? AdvertismentId { get; set; }
        public string AdvertismentTitle { get; set; }
        public string? VoipUniqueNumber { get; set; }
        public string? CustomerConnectedNumber { get; set; }
        public string? SmsUniqueNumber { get; set; }
        public int? CustomerId { get; set; }
        public int? EmployeeId { get; set; }
        public DateTime CreatedAt { get; set; }
        public virtual string CustomerFullName { get; set; } = default!;
        public virtual string CustomerFirstName { get; set; } = default!;
        public virtual string CustomerLastName { get; set; } = default!;
        public string ExtentionNumber { get; set; }
        public string EmployeeFullName { get; set; }
        public string EmployeeUnitPositionTitle { get; set; }
        public bool HasTask { get; set; }
        public int TaskId { get; set; }

        public void Mapping(Profile profile)
        {

            profile.CreateMap<Domain.Entities.Communication, CommunicationModel>()
                .ForMember(x => x.CreatedAt, opt => opt.MapFrom(x => x.CreatedAt))
                .ForMember(x => x.EmployeeUnitPositionTitle, opt => opt.MapFrom(x => x.Employee.UnitPosition.Unit.Title + " " + x.Employee.UnitPosition.Position.Title))
                .ForMember(x => x.CustomerFullName, opt => opt.MapFrom(x => x.Customer.Person.FirstName ?? "" + " " + x.Customer.Person.LastName ?? ""))
                .ForMember(x => x.CustomerFirstName, opt => opt.MapFrom(x => x.Customer.Person.FirstName ?? ""))
                .ForMember(x => x.CustomerLastName, opt => opt.MapFrom(x => x.Customer.Person.LastName ?? ""))
                .ForMember(x => x.EmployeeFullName, opt => opt.MapFrom(x => x.Employee.Person.FirstName + " " + x.Employee.Person.LastName ?? ""))
                .ForMember(x => x.ExtentionNumber, opt => opt.MapFrom(x => x.Employee.Operator.ExtentionNumber ?? ""))
                .ForMember(x => x.TypeBaseTitle, opt => opt.MapFrom(x => x.TypeBase.Title ?? ""))
                .ForMember(x => x.TypeBaseUniqueName, opt => opt.MapFrom(x => x.TypeBase.UniqueName ?? ""))
                .ForMember(x => x.HasTask, opt => opt.MapFrom(x => x.Tasks.Any()))
                .ForMember(x => x.TaskId, opt => opt.MapFrom(x => x.Tasks.Select(t=>t.Id).FirstOrDefault()))
                ;
        }
    }
}