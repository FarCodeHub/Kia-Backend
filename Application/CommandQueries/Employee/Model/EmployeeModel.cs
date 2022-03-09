using System;
using Application.CommandQueries.Person.Model;
using AutoMapper;
using Infrastructure.Mappings;

namespace Application.CommandQueries.Employee.Model
{
    public class EmployeeModel : IMapFrom<Domain.Entities.Employee>, IMapFrom<Domain.Entities.Person>
    {
        public int Id { get; set; }

        /// <summary>
        /// کد پرسنلی
        /// </summary>
        public int PersonId { get; set; } = default!;

        /// <summary>
        /// کد موقعیت واحد
        /// </summary>
        public int UnitPositionId { get; set; } = default!;

        /// <summary>
        /// کد پرسنلی
        /// </summary>
        public string EmployeeCode { get; set; } = default!;

        /// <summary>
        /// تاریخ استخدام
        /// </summary>
        public DateTime EmploymentDate { get; set; } = default!;


        /// <summary>
        /// تاریخ ترک کار
        /// </summary>
        public DateTime? LeaveDate { get; set; }


        public int BranchId { get; set; }
        public string BranchTitle { get; set; }
        public int UnitId { get; set; }
        public string UnitPositionTitle { get; set; }
        public string PositionUniqueName { get; set; }
        public PersonModel PersonModel { get; set; }
        public string? ExtentionNumber { get; set; } = default!;
        public bool? IsOperator { get; set; } = false;

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.Employee, EmployeeModel>()
                .ForMember(x => x.PersonModel, opt => opt.MapFrom(x => x.Person))
                .ForMember(x => x.UnitPositionTitle, opt => opt.MapFrom(x => $"{x.UnitPosition.Unit.Title}-{x.UnitPosition.Position.Title}"))
                .ForMember(x => x.PositionUniqueName, opt => opt.MapFrom(x => x.UnitPosition.Position.UniqueName))
                .ForMember(x => x.UnitId, opt => opt.MapFrom(x => x.UnitPosition.UnitId))
                .ForMember(x => x.BranchId, opt => opt.MapFrom(x => x.UnitPosition.Unit.BranchId))
                .ForMember(x => x.BranchTitle, opt => opt.MapFrom(x => x.UnitPosition.Unit.Branch.Title))
                .ForMember(x => x.ExtentionNumber, opt => opt.MapFrom(x => x.Operator.ExtentionNumber))
                .ForMember(x => x.IsOperator, opt => opt.MapFrom(x => x.Operator.IsActive))
                ;

            profile.CreateMap<Domain.Entities.Person, EmployeeModel>().IgnoreAllNonExisting()
                ;
        }
    }

}
