using System.Collections;
using ChapsDotNET.Business.Models;
using ChapsDotNET.Business.Models.Common;

namespace ChapsDotNET.Business.Interfaces
{
    public interface IMPComponent
    {
        Task<PagedResult<List<MPModel>>> GetMPsAsync(MPRequestModel request);
        Task<int> AddMPAsync(MPModel model);
        Task UpdateMPAsync(MPModel model);
        Task<MPModel> GetMPAsync(int id);
    }
}
