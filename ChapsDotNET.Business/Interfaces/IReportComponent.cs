using System;
using System.Threading.Tasks;
using ChapsDotNET.Business.Models;
using ChapsDotNET.Business.Models.Common;

namespace ChapsDotNET.Business.Interfaces
{
    public interface IReportComponent
    {
        Task<ReportModel> GetReportAsync(int id);
        Task UpdateReportAsync(ReportModel model);
        Task<List<ReportModel>> GetReportsAsync();
    }
}
