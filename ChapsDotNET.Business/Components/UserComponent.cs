using ChapsDotNET.Business.Exceptions;
using ChapsDotNET.Business.Interfaces;
using ChapsDotNET.Business.Models;
using ChapsDotNET.Business.Models.Common;
using ChapsDotNET.Data.Contexts;
using ChapsDotNET.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChapsDotNET.Business.Components
{
    public class UserComponent : IUserComponent
    {
        private readonly DataContext _context;

        public UserComponent(DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// This method by default returns a list of Users
        /// </summary>
        /// <param name="request"></param>
        /// <returns>A list of UserModel</returns>
        public async Task<List<UserModel>> GetUsersAsync(string? sortOrder = null)
        {
            var query = _context.Users.Include(u => u.Team).Include(u => u.Role).AsQueryable();

            switch (sortOrder)
            {
                case "UserLogin":
                    query = query.OrderBy(x => x.Name);
                    break;
                case "UserLogin_desc":
                    query = query.OrderByDescending(x => x.Name);
                    break;
                case "Display_name":
                    query = query.OrderBy(x => x.DisplayName);
                    break;
                case "Display_name_desc":
                    query = query.OrderByDescending(x => x.DisplayName);
                    break;
                case "Team":
                    query = query.OrderBy(x => x.Team != null ? x.Team.Name : string.Empty);
                    break;
                case "Team_desc":
                    query = query.OrderByDescending(x => x.Team != null ? x.Team.Name : string.Empty);
                    break;
                case "Access_Level":
                    query = query.OrderBy(x => x.Role != null ? x.Role.Detail : string.Empty);
                    break;
                case "Access_Level_desc":
                    query = query.OrderByDescending(x => x.Role != null ? x.Role.Detail : string.Empty);
                    break;
                case "email":
                    query = query.OrderBy(x => x.email);
                    break;
                case "email_desc":
                    query = query.OrderByDescending(x => x.email);
                    break;
                case "Last_Active":
                    query = query.OrderBy(x => x.LastActive);
                    break;
                case "Last_Active_desc":
                    query = query.OrderByDescending(x => x.LastActive);
                    break;
                default:
                    query = query.OrderBy(x => x.UserID); 
                    break;
            }

            


            //Row Count
            var count = await query.CountAsync();

            var UserList = await query
                .Select(x => new UserModel
                {
                    UserId = x.UserID,
                    Name = x.Name,
                    DisplayName = x.DisplayName,
                    Email = x.email,
                    RoleStrength = x.RoleStrength,
                    TeamId = x.TeamID,
                    LastActive = x.LastActive,
                    Team = x.Team != null? x.Team.Name : string.Empty,
                    Role = x.Role != null? x.Role.Detail : string.Empty
                }).ToListAsync();

            return UserList;
            
        }

        public async Task<int> AddUserAsync(UserModel model)   
        {
            if (string.IsNullOrEmpty(model.Name))
            {
                throw new ArgumentNullException("Parameter Name cannot be empty");
            }

            var user = new User
            {
                Name = model.Name,
                DisplayName = model.DisplayName == null ? string.Empty : model.DisplayName,
                email = model.Email,
                TeamID = model.TeamId,
                RoleStrength = model.RoleStrength,  
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user.UserID;
        }

        public async Task<bool> IsUserAuthorisedAsync(string? userEmailAddress)
        {
            //If User was never authenticated then we expect to get null
            if (userEmailAddress == null) return false;

            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Name == userEmailAddress && x.RoleStrength > 0);

            return user != null;
        }

        public async Task<UserModel> GetUserByNameAsync(string? userEmailAddress)
        {
            if (string.IsNullOrEmpty(userEmailAddress?.Trim()))
                throw new ArgumentNullException("UserEmailAddress cannot be null");

            var user = await _context.Users
                .Include(x => x.Team)
                .FirstOrDefaultAsync(x => x.Name == userEmailAddress);

            if (user == null)
                throw new NotFoundException("User", userEmailAddress);

            return new UserModel
            {
                Name = user.Name,
                DisplayName = user.DisplayName,
                RoleStrength = user.RoleStrength,
                Email = (user.email ?? string.Empty),
                UserId = user.UserID,
                TeamId = user.TeamID,
                TeamAcronym = user.Team?.Acronym ?? string.Empty,
            };
        }
    }
}
