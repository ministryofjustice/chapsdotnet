using ChapsDotNET.Business.Models;
using ChapsDotNET.Business.Models.Common;

public class IndexViewModel
{
    public PagedResult<List<MPModel>>? MPs { get; set; }
    public PaginationInfo? Pagination { get; set; }
}

