namespace Domain.Entities
{
    public partial class ContractProject : BaseEntity
    {
        public int ContractId { get; set; }
        public int ProjectId { get; set; }

        public virtual Project Project { get; set; }
        public virtual Contract Contract { get; set; }
        public virtual User CreatedBy { get; set; }
        public virtual User ModifiedBy { get; set; }
        public virtual Role OwnerRole { get; set; }
    }
}
