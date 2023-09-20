using System;
using ChapsDotNET.Business.Models;
using ChapsDotNET.Business.Models.Common;

namespace ChapsDotNET.Business.Interfaces
{
	public interface IAlertComponent
	{
        Task <List<AlertModel>> GetAlertsAsync(AlertRequestModel request);
        Task<AlertModel> GetAlertAsync(int Id);
        Task<int> AddAlertAsync(AlertModel model);
        Task UpdateAlertAsync(AlertModel model);
        Task<List<AlertModel>> GetCurrentAlertsAsync();

    }
}