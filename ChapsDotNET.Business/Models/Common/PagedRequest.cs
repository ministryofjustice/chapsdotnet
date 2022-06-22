namespace ChapsDotNET.Business.Models.Common
{
    public abstract class PagedRequest
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public bool NoPaging { get; set; }
    }
}
