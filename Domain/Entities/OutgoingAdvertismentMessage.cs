namespace Domain.Entities
{
    public partial class OutgoingAdvertismentMessage : BaseEntity
    {
        public int AdvertismentSourceId { get; set; } = default!;
        public string Reciver { get; set; } = default!;

        public virtual Advertisement AdvertismentSource { get; set; } = default!;
        public virtual User CreatedBy { get; set; } = default!;
        public virtual User? ModifiedBy { get; set; } = default!;
    }
}
