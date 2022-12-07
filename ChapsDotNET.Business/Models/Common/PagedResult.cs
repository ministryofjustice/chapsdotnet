namespace ChapsDotNET.Business.Models.Common
{
    public class PagedResult<T> : IPagedResult
    {
        public T? Results { get; set; }
        public int CurrentPage { get; set; }
        public int PageCount => (int)Math.Ceiling((double)RowCount / PageSize);
        public int PageSize { get; set; }
        public int RowCount { get; set; }
        //public string? sortOrder { get; set; }
        //public string? nameFilterTerm { get; set; }
        //public string? addressFilterTerm { get; set; }
        //public string? emailFilterTerm { get; set; }
        public bool activeFilter { get; set; }
    }

    public interface IPagedResult
    {
        public int CurrentPage { get; set; }
        public int PageCount { get; }
        public int PageSize { get; set; }
        public int RowCount { get; set; }
    }
}
