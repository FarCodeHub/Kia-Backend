namespace Domain.Entities
{
    public partial class MenuItem : BaseEntity
    {
        public int? PermissionId { get; set; }
        public int? ParentId { get; set; }
        public string? Title { get; set; }
        public string? Icon { get; set; }
        public string? Link { get; set; }
        public string? SubTitle { get; set; }
        public bool? Active { get; set; }
        public bool? Disable { get; set; }
        public int OrderIndex { get; set; }
        public virtual User CreatedBy { get; set; } = default!;
        public virtual User? ModifiedBy { get; set; } = default!;
        public virtual Role OwnerRole { get; set; } = default!;
        public virtual Permission? Permission { get; set; } = default!;
    }
}
