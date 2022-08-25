using System;
using System.Threading.Tasks;
using ChapsDotNET.Business.Models;
using ChapsDotNET.Business.Models.Common;

namespace ChapsDotNET.Business.Interfaces
{
	public interface IMoJMinisterComponent
	{
        Task<PagedResult<List<MoJMinisterModel>>> GetMoJMinistersAsync(MoJMinisterRequestModel request);
        Task<int> AddMoJMinisterAsync(MoJMinisterModel model);
        Task UpdateMoJMinisterAsync(MoJMinisterModel model);
        Task<MoJMinisterModel> GetMoJMinisterAsync(int id);
    }
}
