using ChapsDotNET.Business.Models;
using ChapsDotNET.Business.Models.Common;

namespace ChapsDotNET.Business.Interfaces
{
    public interface ILeadSubjectComponent
    {
        Task<PagedResult<List<LeadSubjectModel>>> GetLeadSubjectsAsync(LeadSubjectRequestModel request);
        Task<int> AddLeadSubjectAsync(LeadSubjectModel model);
        Task UpdateLeadSubjectAsync(LeadSubjectModel model);
        Task<LeadSubjectModel> GetLeadSubjectAsync(int id);
    }
}