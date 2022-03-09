namespace Domain.Entities
{
    public partial class UserSetting : BaseEntity
    {
        public int UserId { get; set; } = default!;
        public string KeyWord { get; set; } = default!;
        public string Value { get; set; } = default!;


        public virtual User CreatedBy { get; set; } = default!;
        public virtual User? ModifiedBy { get; set; } = default!;
        public virtual Role OwnerRole { get; set; } = default!;
        public virtual User User { get; set; } = default!;
    }
}
