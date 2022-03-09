using System.Collections.Generic;
using System.Linq;
using Application.CommandQueries.Person.Model;
using Application.CommandQueries.Role.Model;
using AutoMapper;
using Infrastructure.Mappings;

namespace Application.CommandQueries.User.Model
{
    public class UserModel : IMapFrom<Domain.Entities.User>
    {
        public int Id { get; set; }
        public int PersonId { get; set; } = default!;
        public string Username { get; set; } = default!;
        public bool IsLoggedIn { get; set; } = default!;
        public bool IsBlocked { get; set; } = default!;
        public string? BlockedReason { get; set; }
        public int FailedCount { get; set; } = default!;
        public ICollection<int> RolesIdList { get; set; }
        public string RoleTitle { get; set; }
        public string FirstName { get; set; }
        public string FullName { get; set; }
        public string LastName { get; set; }
        public string NationalNumber { get; set; }
        public string UnitPositionTitle { get; set; }
        public PersonModel Person { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.User, UserModel>()
                .ForMember(x => x.UnitPositionTitle, opt => opt.MapFrom(x => x.Person.Employee.UnitPosition.Unit.Title + "-" + x.Person.Employee.UnitPosition.Position.Title))
                .ForMember(x => x.Person, opt => opt.MapFrom(x => x.Person))
                .ForMember(x => x.FirstName, opt => opt.MapFrom(x => x.Person.FirstName))
                .ForMember(x => x.FullName, opt => opt.MapFrom(x => x.Person.FirstName + " " + x.Person.LastName))
                .ForMember(x => x.NationalNumber, opt => opt.MapFrom(x => x.Person.NationalCode))
                .ForMember(x => x.LastName, opt => opt.MapFrom(x => x.Person.LastName))
                .ForMember(x => x.RoleTitle, opt => opt.MapFrom(x => x.UserRoleUsers.FirstOrDefault().Role.Title))
                .ForMember(x => x.RolesIdList, opt => opt.MapFrom(x => x.UserRoleUsers.Select(t => t.RoleId)))
                ;
        }
    }
}
