using System;
using System.Linq;
using AutoMapper;
using Infrastructure.Mappings;

namespace Application.CommandQueries.Commission.Model
{
    public class CommissionModel : IMapFrom<Domain.Entities.Commission>
    {
        public int Id { get; set; }
        public int ContractId { get; set; } = default!;
        public long Amount { get; set; } = default!;
        public int EmployeeId { get; set; } = default!;
        public string EmployeeFullName { get; set; } = default!;
        public bool IsPaid { get; set; } = default!;
        public DateTime? PaidAt { get; set; }
        public int ProjectId { get; set; }
        public string ProjectTitle { get; set; }
        public DateTime CreatedAt { get; set; }

        public string BranchTitle { get; set; }
        public string UnitPositionTitle { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.Commission, CommissionModel>()
                .ForMember(x => x.ProjectId, opt => opt.MapFrom(x => x.Contract.ContractProjects.FirstOrDefault().Project.Id))
                .ForMember(x => x.ProjectTitle, opt => opt.MapFrom(x => x.Contract.ContractProjects.FirstOrDefault().Project.Title))
                .ForMember(x => x.EmployeeFullName, opt => opt.MapFrom(x => x.Employee.Person.FirstName + " " + x.Employee.Person.LastName))
                .ForMember(x => x.CreatedAt, opt => opt.MapFrom(x => x.CreatedAt))
                .ForMember(x => x.UnitPositionTitle, opt => opt.MapFrom(x => $"{x.Employee.UnitPosition.Unit.Title}-{x.Employee.UnitPosition.Position.Title}"))
                .ForMember(x => x.BranchTitle, opt => opt.MapFrom(x => x.Employee.UnitPosition.Unit.Branch.Title))
                ;
        }
    }
}
