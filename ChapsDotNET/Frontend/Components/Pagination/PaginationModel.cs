namespace ChapsDotNET.Frontend.Components.Pagination
{
    public class PaginationModel
    {
        public static int GetResultsFrom(int CurrentPage, int PageSize )
        {
            return ((CurrentPage - 1) * PageSize) + 1;
        }

        public static int GetResultsTo(int CurrentPage, int PageSize, int ResultCount)
        {
            return ((CurrentPage - 1) * PageSize) + ResultCount;
        }

        public int CurrentPage { get; set; }
        public int TotalCount { get; set; }
        public int PageSize { get; set; } = 10;
        public int PageCount { get; set; }
    }
}
