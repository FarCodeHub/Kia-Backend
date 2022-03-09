namespace Domain.Entities
{
    public partial class ValidationMessage : BaseEntity
    {
        public string KeyWord { get; set; } = default!;
        public string Message { get; set; } = default!;


        public virtual User CreatedByIdNavigation { get; set; } = default!;
        public virtual User ModifiedByIdNavigation { get; set; } = default!;
        public virtual Role OwnerRole { get; set; } = default!;
    }
}
