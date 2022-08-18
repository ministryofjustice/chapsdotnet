using ChapsDotNET.Business.Models;
using ChapsDotNET.Business.Models.Common;

namespace ChapsDotNET.Business.Interfaces
{
    public interface ICampaignComponent
    {
        Task<PagedResult<List<CampaignModel>>> GetCampaignsAsync(CampaignRequestModel request);
        Task<int> AddCampaignAsync(CampaignModel model);
        Task UpdateCampaignAsync(CampaignModel model);
        Task<CampaignModel> GetCampaignAsync(int id);
    }
}
