using ChapsDotNET.Business.Models;
using ChapsDotNET.Business.Models.Common;
using ChapsDotNET.Data.Entities;

namespace ChapsDotNET.Business.Interfaces
{
    public interface ISalutationComponent
    {
        Task<PagedResult<List<SalutationModel>>> GetSalutationsAsync(SalutationRequestModel request);
        Task <string> AddSalutation(SalutationModel model);
        void UpdateSalutationActiveStatus(int id, bool state);
    }
}
