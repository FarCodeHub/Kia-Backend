using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class User : BaseEntity
    {
    
        public int PersonId { get; set; } = default!;
        public string Username { get; set; } = default!;
        public bool IsBlocked { get; set; } = default!;
        public string? BlockedReason { get; set; }
        public string Password { get; set; } = default!;
        public int FailedCount { get; set; } = default!;

        public virtual Person Person { get; set; } = default!;
        public virtual ICollection<Census> Census { get; set; }

        public virtual ICollection<ContractProject> ContractProjectCreatedBies { get; set; }
        public virtual ICollection<ContractProject> ContractProjectModifiedBies { get; set; }
        public virtual ICollection<Advertisement> AdvertisementCreatedBies { get; set; } = default!;
        public virtual ICollection<Advertisement> AdvertisementModifiedBies { get; set; } = default!;
        public virtual ICollection<BaseValue> BaseValueCreatedBies { get; set; } = default!;
        public virtual ICollection<BaseValue> BaseValueModifiedBies { get; set; } = default!;
        public virtual ICollection<BaseValueType> BaseValueTypeCreatedBies { get; set; } = default!;
        public virtual ICollection<BaseValueType> BaseValueTypeModifiedBies { get; set; } = default!;
        public virtual ICollection<Branch> BranchCreatedBies { get; set; } = default!;
        public virtual ICollection<Branch> BranchModifiedBies { get; set; } = default!;
        public virtual ICollection<Commission> CommissionCreatedBies { get; set; } = default!;
        public virtual ICollection<Commission> CommissionModifiedBies { get; set; } = default!;
        public virtual ICollection<Communication> CommunicationCreatedBies { get; set; } = default!;
        public virtual ICollection<Communication> CommunicationModifiedBies { get; set; } = default!;
        public virtual ICollection<ContractAttachment> ContractAttachmentCreatedBies { get; set; } = default!;
        public virtual ICollection<ContractAttachment> ContractAttachmentModifiedBies { get; set; } = default!;
        public virtual ICollection<Contract> ContractCreatedBies { get; set; } = default!;
        public virtual ICollection<Contract> ContractModifiedBies { get; set; } = default!;
        public virtual ICollection<Customer> CustomerCreatedBies { get; set; } = default!;
        public virtual ICollection<Customer> CustomerModifiedBies { get; set; } = default!;
        public virtual ICollection<Employee> EmployeeCreatedBies { get; set; } = default!;
        public virtual ICollection<Employee> EmployeeModifiedBies { get; set; } = default!;
        public virtual ICollection<MenuItem> MenuItemCreatedBies { get; set; } = default!;
        public virtual ICollection<MenuItem> MenuItemModifiedBies { get; set; } = default!;
        public virtual ICollection<Operator> OperatorCreatedBies { get; set; } = default!;
        public virtual ICollection<Operator> OperatorModifiedByNavigations { get; set; } = default!;
        public virtual ICollection<OutgoingAdvertismentMessage> OutgoingAdvertismentMessageCreatedBies { get; set; } = default!;
        public virtual ICollection<OutgoingAdvertismentMessage> OutgoingAdvertismentMessageModifiedBies { get; set; } = default!;
        public virtual ICollection<Permission> PermissionCreatedBies { get; set; } = default!;
        public virtual ICollection<Permission> PermissionModifiedBies { get; set; } = default!;
        public virtual ICollection<Project> ProjectCreatedBies { get; set; } = default!;
        public virtual ICollection<Project> ProjectModifiedBies { get; set; } = default!;
        public virtual ICollection<RolePermission> RolePermissionCreatedBies { get; set; } = default!;
        public virtual ICollection<RolePermission> RolePermissionModifiedBies { get; set; } = default!;
        public virtual ICollection<SessionSurvey> SessionSurveyCreatedBies { get; set; } = default!;
        public virtual ICollection<SessionSurvey> SessionSurveyModifiedBies { get; set; } = default!;
        public virtual ICollection<Task> TaskCreatedBies { get; set; } = default!;
        public virtual ICollection<Task> TaskModifiedBies { get; set; } = default!;
        public virtual ICollection<Unit> UnitCreatedBies { get; set; } = default!;
        public virtual ICollection<Unit> UnitModifiedBies { get; set; } = default!;
        public virtual ICollection<UserRole> UserRoleCreatedBies { get; set; } = default!;
        public virtual ICollection<UserRole> UserRoleModifiedBies { get; set; } = default!;
        public virtual ICollection<UserRole> UserRoleUsers { get; set; } = default!;
        public virtual ICollection<UserSetting> UserSettingCreatedBies { get; set; } = default!;
        public virtual ICollection<UserSetting> UserSettingModifiedBies { get; set; } = default!;
        public virtual ICollection<UserSetting> UserSettingUsers { get; set; } = default!;
        public virtual ICollection<Case> CasCreatedBies { get; set; }
        public virtual ICollection<Case> CasModifiedBies { get; set; }
        public virtual ICollection<CaseEmployeeChange> CaseEmployeeChanxCreatedBies { get; set; }
        public virtual ICollection<CaseEmployeeChange> CaseEmployeeChanxModifiedBies { get; set; }
        public virtual ICollection<CountryDivision> CountryDivisionCreatedBies { get; set; }
        public virtual ICollection<CountryDivision> CountryDivisionModifiedBies { get; set; }
    }
}
