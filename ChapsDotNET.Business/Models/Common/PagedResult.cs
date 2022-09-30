namespace ChapsDotNET.Business.Models.Common
{
    public class PagedResult<T> : IPagedResult
    {
        public T? Results { get; set; }
        public int CurrentPage { get; set; }
        public int PageCount => (int)Math.Ceiling((double)RowCount / PageSize);
        public int PageSize { get; set; }
        public int RowCount { get; set; }
    }

    public interface IPagedResult
    {
        public int CurrentPage { get; set; }
        public int PageCount { get; }
        public int PageSize { get; set; }
        public int RowCount { get; set; }
    }
}
