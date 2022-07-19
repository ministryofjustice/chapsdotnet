using ChapsDotNET.Business.Models;

namespace ChapsDotNET.Business.Interfaces
{
    public interface IUserComponent
    {
        public Task<bool> IsUserAuthorisedAsync(string? userEmailAddress);
        public Task<UserModel> GetUserByNameAsync(string? userEmailAddress);
    }


}

