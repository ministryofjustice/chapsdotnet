using ChapsDotNET.Business.Models;
using ChapsDotNET.Business.Models.Common;

namespace ChapsDotNET.Business.Interfaces
{
    public interface IUserComponent
    {
        public Task<bool> IsUserAuthorisedAsync(string? userEmailAddress);
        public Task<UserModel> GetUserByNameAsync(string? userEmailAddress);
        public Task<List<UserModel>> GetUsersAsync(string? sortOrder);
        public Task<int> AddUserAsync(UserModel model);
    }
}
