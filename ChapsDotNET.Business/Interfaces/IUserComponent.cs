using ChapsDotNET.Business.Models;
using ChapsDotNET.Business.Models.Common;

namespace ChapsDotNET.Business.Interfaces
{
    public interface IUserComponent
    {
        public Task<bool> IsUserAuthorisedAsync(string? userEmailAddress);
        public Task<UserModel> GetUserByNameAsync(string? userEmailAddress);
        Task<PagedResult<List<UserModel>>> GetUsersAsync(UserRequestModel request);
        public Task<(int userId, string warning)> AddUserAsync(UserModel model);
        public Task<UserModel> GetUserByIdAsync(int userId);
        public Task UpdateUserAsync(UserModel model);
    }
}
