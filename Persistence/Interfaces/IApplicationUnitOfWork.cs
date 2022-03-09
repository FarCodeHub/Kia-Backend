using Domain.Entities;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Interfaces
{
    public interface IApplicationUnitOfWork : IUnitOfWork
    {
        DbSet<Advertisement> Advertisements { get; set; }
        DbSet<BaseValue> BaseValues { get; set; }
        DbSet<BaseValueType> BaseValueTypes { get; set; }
        DbSet<Branch> Branches { get; set; }
        DbSet<Census> Censuses { get; set; }

        DbSet<ContractProject> ContractProjects { get; set; }
        DbSet<Commission> Commissions { get; set; }
        DbSet<Communication> Communications { get; set; }
        DbSet<Contract> Contracts { get; set; }
        DbSet<ContractAttachment> ContractAttachments { get; set; }
        DbSet<CountryDivision> CountryDivisions { get; set; }
        DbSet<Customer> Customers { get; set; }
        DbSet<Employee> Employees { get; set; }
        DbSet<MenuItem> MenuItems { get; set; }
        DbSet<Operator> Operators { get; set; }
        DbSet<OutgoingAdvertismentMessage> OutgoingAdvertismentMessages { get; set; }
        DbSet<Permission> Permissions { get; set; }
        DbSet<Person> Persons { get; set; }
        DbSet<Position> Positions { get; set; }
        DbSet<Project> Projects { get; set; }
        DbSet<Question> Questions { get; set; }
        DbSet<Role> Roles { get; set; }
        DbSet<RolePermission> RolePermissions { get; set; }
        DbSet<SessionSurvey> SessionSurveys { get; set; }
        DbSet<Domain.Entities.Task> Tasks { get; set; }
        DbSet<Unit> Units { get; set; }
        DbSet<UnitPosition> UnitPositions { get; set; }
        DbSet<User> Users { get; set; }
        DbSet<UserRole> UserRoles { get; set; }
        DbSet<UserSetting> UserSettings { get; set; }
        DbSet<Case> Cases { get; set; }
        DbSet<CaseEmployeeChange> CaseEmployeeChanges { get; set; }
        DbSet<Configuration> Configurations { get; set; }



    }
}