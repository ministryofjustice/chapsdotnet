using ChapsDotNET.Business.Interfaces;
using ChapsDotNET.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ChapsDotNET.Business.Components
{
    public class UserComponent : IUserComponent
    {
        private readonly DataContext _context;

        public UserComponent(DataContext context)
        {
            this._context = context;
        }

        public async Task<bool> IsUserAuthorisedAsync(string? userEmailAddress)
        {
            //If User was never authenticated then we expect to get null
            if (userEmailAddress == null) return false;

            var user =  await _context.Users
                .FirstOrDefaultAsync(x => x.Name == userEmailAddress && x.RoleStrength > 0);

            return user != null;
        }
    }
}

