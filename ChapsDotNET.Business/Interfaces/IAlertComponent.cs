using ChapsDotNET.Business.Models;
using ChapsDotNET.Business.Models.Common;

namespace ChapsDotNET.Business.Interfaces
{
    public interface IAlertComponent
    {
        Task<PagedResult<List<AlertModel>>> GetAlertsAsync(AlertRequestModel request);
        Task<int> AddAlertAsync(AlertModel model);
        Task UpdateAlertAsync(AlertModel model);
        Task<AlertModel> GetAlertAsync(int id);
    }
}