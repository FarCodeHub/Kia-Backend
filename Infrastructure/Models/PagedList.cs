namespace Infrastructure.Models
{
    public class PagedList<T> : IPagedList<T>
    {
        public T Data { get; set; }
        public int TotalCount { get; set; }
    }

    public interface IPagedList<T>
    {
        public T Data { get; set; }
        public int TotalCount { get; set; }
    }
}