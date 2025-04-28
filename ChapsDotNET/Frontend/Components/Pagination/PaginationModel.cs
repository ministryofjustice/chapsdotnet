namespace ChapsDotNET.Frontend.Components.Pagination
{
    public class PaginationModel
    {
        public static List<T> GetPagedResults<T>(List<T> List, int PageSize, int CurrentPage)
        {
            return List.Skip(CurrentPage * PageSize).Take(PageSize).ToList();
        }

        public int GetTotalPages(double TotalCount, double PageSize)
        {
            var pages = TotalCount / PageSize;
            return (int)Math.Ceiling(pages);
        }

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
    }
}
