namespace Infrastructure.Interfaces
{
    public interface  IHierarchical : IBaseEntity
    {
        public string LevelCode { get; set; }
        public int? ParentId { get; set; }
    }
}