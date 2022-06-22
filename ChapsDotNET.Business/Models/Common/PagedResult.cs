namespace ChapsDotNET.Business.Models.Common
{
    public class PagedResult<T>
    {
        public T Results { get; set; }
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
        public int PageSize { get; set; }
        public int RowCount { get; set; }
    }
}
