using Microsoft.EntityFrameworkCore;

namespace Domain.Entities
{
    public partial class KiaautomationContext : DbContext
    {
        public virtual DbSet<Advertisement> Advertisements { get; set; } = default!;
        public virtual DbSet<BaseValue> BaseValues { get; set; } = default!;
        public virtual DbSet<BaseValueType> BaseValueTypes { get; set; } = default!;
        public virtual DbSet<Branch> Branches { get; set; } = default!;
        public virtual DbSet<Commission> Commissions { get; set; } = default!;
        public virtual DbSet<Communication> Communications { get; set; } = default!;
        public virtual DbSet<Contract> Contracts { get; set; } = default!;
        public virtual DbSet<ContractAttachment> ContractAttachments { get; set; } = default!;
        public virtual DbSet<CountryDivision> CountryDivisions { get; set; } = default!;
        public virtual DbSet<Customer> Customers { get; set; } = default!;
        public virtual DbSet<Employee> Employees { get; set; } = default!;
        public virtual DbSet<MenuItem> MenuItems { get; set; } = default!;
        public virtual DbSet<Operator> Operators { get; set; } = default!;
        public virtual DbSet<OutgoingAdvertismentMessage> OutgoingAdvertismentMessages { get; set; } = default!;
        public virtual DbSet<Permission> Permissions { get; set; } = default!;
        public virtual DbSet<Person> Persons { get; set; } = default!;
        public virtual DbSet<Position> Positions { get; set; } = default!;
        public virtual DbSet<Project> Projects { get; set; } = default!;
        public virtual DbSet<Question> Questions { get; set; } = default!;
        public virtual DbSet<Role> Roles { get; set; } = default!;
        public virtual DbSet<RolePermission> RolePermissions { get; set; } = default!;
        public virtual DbSet<SessionSurvey> SessionSurveys { get; set; } = default!;
        public virtual DbSet<Task> Tasks { get; set; } = default!;
        public virtual DbSet<Unit> Units { get; set; } = default!;
        public virtual DbSet<UnitPosition> UnitPositions { get; set; } = default!;
        public virtual DbSet<User> Users { get; set; } = default!;
        public virtual DbSet<UserRole> UserRoles { get; set; } = default!;
        public virtual DbSet<UserSetting> UserSettings { get; set; } = default!;

        public KiaautomationContext(DbContextOptions<KiaautomationContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.ApplyConfiguration(new reverse.Configurations.AdvertisementConfiguration());
            modelBuilder.ApplyConfiguration(new reverse.Configurations.BaseValueConfiguration());
            modelBuilder.ApplyConfiguration(new reverse.Configurations.BaseValueTypeConfiguration());
            modelBuilder.ApplyConfiguration(new reverse.Configurations.BranchConfiguration());
            modelBuilder.ApplyConfiguration(new reverse.Configurations.CommissionConfiguration());
            modelBuilder.ApplyConfiguration(new reverse.Configurations.CommunicationConfiguration());
            modelBuilder.ApplyConfiguration(new reverse.Configurations.ContractConfiguration());
            modelBuilder.ApplyConfiguration(new reverse.Configurations.ContractAttachmentConfiguration());
            modelBuilder.ApplyConfiguration(new reverse.Configurations.CountryDivisionConfiguration());
            modelBuilder.ApplyConfiguration(new reverse.Configurations.CustomerConfiguration());
            modelBuilder.ApplyConfiguration(new reverse.Configurations.EmployeeConfiguration());
            modelBuilder.ApplyConfiguration(new reverse.Configurations.MenuItemConfiguration());
            modelBuilder.ApplyConfiguration(new reverse.Configurations.OperatorConfiguration());
            modelBuilder.ApplyConfiguration(new reverse.Configurations.OutgoingAdvertismentMessageConfiguration());
            modelBuilder.ApplyConfiguration(new reverse.Configurations.PermissionConfiguration());
            modelBuilder.ApplyConfiguration(new reverse.Configurations.PersonConfiguration());
            modelBuilder.ApplyConfiguration(new reverse.Configurations.PositionConfiguration());
            modelBuilder.ApplyConfiguration(new reverse.Configurations.ProjectConfiguration());
            modelBuilder.ApplyConfiguration(new reverse.Configurations.QuestionConfiguration());
            modelBuilder.ApplyConfiguration(new reverse.Configurations.RoleConfiguration());
            modelBuilder.ApplyConfiguration(new reverse.Configurations.RolePermissionConfiguration());
            modelBuilder.ApplyConfiguration(new reverse.Configurations.SessionSurveyConfiguration());
            modelBuilder.ApplyConfiguration(new reverse.Configurations.TaskConfiguration());
            modelBuilder.ApplyConfiguration(new reverse.Configurations.UnitConfiguration());
            modelBuilder.ApplyConfiguration(new reverse.Configurations.UnitPositionConfiguration());
            modelBuilder.ApplyConfiguration(new reverse.Configurations.UserConfiguration());
            modelBuilder.ApplyConfiguration(new reverse.Configurations.UserRoleConfiguration());
            modelBuilder.ApplyConfiguration(new reverse.Configurations.UserSettingConfiguration());
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
