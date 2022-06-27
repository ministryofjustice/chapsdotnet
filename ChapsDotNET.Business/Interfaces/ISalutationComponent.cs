using ChapsDotNET.Business.Models;
using ChapsDotNET.Business.Models.Common;
using ChapsDotNET.Data.Entities;

namespace ChapsDotNET.Business.Interfaces
{
    public interface ISalutationComponent
    {
        Task<PagedResult<List<SalutationModel>>> GetSalutationsAsync(SalutationRequestModel request);
        Task <int> AddSalutation(SalutationModel model);
        Task UpdateSalutationAsync(int id, bool state);
    }
}
