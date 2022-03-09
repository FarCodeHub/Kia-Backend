using System.Collections.Generic;
using Infrastructure.Interfaces;

namespace Domain.Entities
{
    public partial class Role : BaseEntity, IHierarchical
    {
        public string LevelCode { get; set; } = default!;
        public string Title { get; set; } = default!;
        public string? UniqueName { get; set; }
        public string? Description { get; set; }
        public int? ParentId { get; set; }

        public Role? Parent { get; set; }
        public virtual ICollection<ContractProject> ContractProjects { get; set; }

        public virtual ICollection<Advertisement> Advertisements { get; set; } = default!;
        public virtual ICollection<BaseValueType> BaseValueTypes { get; set; } = default!;
        public virtual ICollection<BaseValue> BaseValues { get; set; } = default!;
        public virtual ICollection<Branch> Branches { get; set; } = default!;
        public virtual ICollection<Commission> Commissions { get; set; } = default!;
        public virtual ICollection<Communication> Communications { get; set; } = default!;
        public virtual ICollection<ContractAttachment> ContractAttachments { get; set; } = default!;
        public virtual ICollection<Contract> Contracts { get; set; } = default!;
        public virtual ICollection<Customer> Customers { get; set; } = default!;
        public virtual ICollection<Employee> Employees { get; set; } = default!;
        public virtual ICollection<MenuItem> MenuItems { get; set; } = default!;
        public virtual ICollection<Operator> Operators { get; set; } = default!;
        public virtual ICollection<Permission> Permissions { get; set; } = default!;
        public virtual ICollection<Person> Persons { get; set; } = default!;
        public virtual ICollection<Position> Positions { get; set; } = default!;
        public virtual ICollection<Project> Projects { get; set; } = default!;
        public virtual ICollection<RolePermission> RolePermissionOwnerRoles { get; set; } = default!;
        public virtual ICollection<RolePermission> RolePermissionRoles { get; set; } = default!;
        public virtual ICollection<SessionSurvey> SessionSurveys { get; set; } = default!;
        public virtual ICollection<Task> Tasks { get; set; } = default!;
        public virtual ICollection<Unit> Units { get; set; } = default!;
        public virtual ICollection<UserRole> UserRoleOwnerRoles { get; set; } = default!;
        public virtual ICollection<UserRole> UserRoleRoles { get; set; } = default!;
        public virtual ICollection<UserSetting> UserSettings { get; set; } = default!;
        public virtual ICollection<Case> Cas { get; set; }
        public virtual ICollection<CaseEmployeeChange> CaseEmployeeChanges { get; set; }
        public virtual ICollection<CountryDivision> CountryDivisions { get; set; }

    }
}
