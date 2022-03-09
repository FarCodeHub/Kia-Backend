namespace Domain.Entities
{
    public partial class ContractAttachment : BaseEntity
    {
        public int ContractId { get; set; } = default!;
        public string FilePath { get; set; } = default!;


        public virtual Contract Contract { get; set; } = default!;
        public virtual User CreatedBy { get; set; } = default!;
        public virtual User? ModifiedBy { get; set; } = default!;
        public virtual Role OwnerRole { get; set; } = default!;
    }
}
