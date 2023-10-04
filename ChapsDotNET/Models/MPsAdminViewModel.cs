using ChapsDotNET.Business.Models;
using ChapsDotNET.Common.Helpers;

public class MPsAdminViewModel
{
    public List<MPModel>? MPs { get; set; }
    public PaginationInfo? Pagination { get; set; }

}

public class PaginationInfo
{
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int TotalItems { get; set; }
    public int ItemsPerPage { get; set; }
}