using System.Collections;
using ChapsDotNET.Business.Models;
using ChapsDotNET.Business.Models.Common;
using Microsoft.AspNetCore.Mvc;

namespace ChapsDotNET.Business.Interfaces
{
    public interface IMPComponent
    {
        Task<PagedResult<List<MPModel>>> GetMPsAsync(MPRequestModel request);
        Task<int> AddMPAsync(MPModel model);
        Task UpdateMPAsync(MPModel model);
        Task<MPModel> GetMPAsync(int id);
        Task<PagedResult<List<MPModel>>> GetFilteredMPsAsync(MPRequestModel model);
    }
}
