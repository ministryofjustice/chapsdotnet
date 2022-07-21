using System;
using System.Threading.Tasks;
using ChapsDotNET.Business.Models;
using ChapsDotNET.Business.Models.Common;

namespace ChapsDotNET.Business.Interfaces
{
    public interface IPublicHolidayComponent
    {
        Task<PagedResult<List<PublicHolidayModel>>> GetPublicHolidaysAsync(PublicHolidayRequestModel request);
        Task<PublicHolidayModel> GetPublicHolidayAsync(int id);
        Task<int> AddPublicHolidayAsync(PublicHolidayModel model);
    }
}
