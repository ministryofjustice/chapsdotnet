using ChapsDotNET.Business.Models;
using ChapsDotNET.Business.Models.Common;

namespace ChapsDotNET.Business.Interfaces
{
    public interface IUserComponent
    {
        public Task<bool> IsUserAuthorisedAsync(string? userEmailAddress);
        public Task<UserModel> GetUserByNameAsync(string? userEmailAddress);
        public Task<List<UserModel>> GetUsersAsync(string? sortOrder);
        public Task<(int userId, string warning)> AddUserAsync(UserModel model);
        public Task<UserModel> GetUserByIdAsync(int userId);
        public Task UpdateUserAsync(UserModel model);
    }
}
