using ChapsDotNET.Business.Models;
using ChapsDotNET.Business.Models.Common;
using ChapsDotNET.Data.Entities;

namespace ChapsDotNET.Business.Interfaces
{
    public interface ISalutationComponent
    {
        Task<PagedResult<List<SalutationModel>>> GetSalutationsAsync(SalutationRequestModel request);
        Task <int> AddSalutationAsync(SalutationModel model);
        Task UpdateSalutationAsync(SalutationModel model);
        Task<SalutationModel> GetSalutationAsync(int id);
    }
}
