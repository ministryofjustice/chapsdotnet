using ChapsDotNET.Business.Models;

namespace ChapsDotNET.Business.Interfaces
{
    public interface IReportComponent
    {
        Task<ReportModel> GetReportAsync(int id);
        Task UpdateReportAsync(ReportModel model);
        Task<List<ReportModel>> GetReportsAsync();
    }
}
