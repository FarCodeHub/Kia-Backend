namespace Infrastructure.Models
{
    public class Pagination
    {
        public Pagination Paginator()
        {
            return this;
        }
        private int _pageIndex;
        private int _pageSize;
        private string _orderByProperty;

        public int PageSize
        {
            get => _pageSize <= 0 ? int.MaxValue : _pageSize;
            set => _pageSize = value > 1000 ? 1000 : value;
        }


        public int PageIndex
        {
            get => _pageIndex <=0 ? 1 : _pageIndex;
            set => _pageIndex = value < 1 ? 1 : value;
        }



        public int Skip => (_pageIndex - 1) * _pageSize;

        public int Take => _pageSize;

        public string OrderByProperty
        {
            get => _orderByProperty ?? "Id desc";
            set => _orderByProperty = string.IsNullOrEmpty(value) ? "Id desc" : value;
        }
    }
}