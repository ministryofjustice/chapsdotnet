﻿using ChapsDotNET.Business.Exceptions;
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
        /// Returns a list of Users
        /// </summary>
        /// <param name="sortOrder">string?</param>
        /// <returns>A paged list of UserModel</returns>
        public async Task<PagedResult<List<UserModel>>> GetUsersAsync(UserRequestModel request)
        {
            var query = _context.Users.Include(u => u.Team).Include(u => u.Role).AsQueryable();

            switch (request.SortOrder)
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

            // Row Count
            var count = await query.CountAsync();

            //Paging query
            if (!request.NoPaging)
            {
                query = query.Skip(((request.PageNumber) - 1) * request.PageSize)
                    .Take(request.PageSize);
            }

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

            return new PagedResult<List<UserModel>>
            {
                Results = UserList,
                PageSize = request.PageSize,
                CurrentPage = request.PageNumber,
                RowCount = count
            };
        }

        /// <summary>
        /// Adds a new user
        /// </summary>
        /// <param name="model">UserModel</param>
        /// <returns>int, string</returns>
        public async Task<(int userId, string warning)> AddUserAsync(UserModel model)   
        {
            if (string.IsNullOrEmpty(model.Name))
            {
                throw new ArgumentNullException("Parameter Name cannot be empty");
            }

            string condensedNewUserName = model.Name.Replace(" ", "").ToLower();
            string duplicateError = "The user you are trying to create already exists. Please amend the existing user details below instead.";

            var users = await GetUsersAsync(new UserRequestModel
            {
                NoPaging = true
            });

            bool nameAndDisplayNameMatches =  users.Results!.Any(x => x.Name!.Equals(model.Name, StringComparison.OrdinalIgnoreCase) && x.DisplayName!.Equals(model.DisplayName, StringComparison.OrdinalIgnoreCase));

            bool loginNameOnlyMatch = users.Results!.Any(x => x.Name!.Equals(model.Name, StringComparison.OrdinalIgnoreCase)
            && !x.DisplayName!.Equals(model.DisplayName, StringComparison.OrdinalIgnoreCase));

            if (nameAndDisplayNameMatches || loginNameOnlyMatch)
            {
                var getuser = await GetUserByLoginNameAsync(model.Name);
                return (getuser.UserId, duplicateError!); 
            }
            else
            {
                var user = new User
                {
                    Name = model.Name,
                    DisplayName = model.DisplayName == null ? string.Empty : model.DisplayName,
                    email = model.Email,
                    TeamID = model.TeamId,
                    RoleStrength = model.RoleStrength,
                };
                var success = "Success";
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                return (user.UserID, success);
            }
        }

        /// <summary>
        /// Checks if current user is authorised
        /// </summary>
        /// <param name="userEmailAddress">string?</param>
        /// <returns>bool</returns>
        public async Task<bool> IsUserAuthorisedAsync(string? userEmailAddress)
        {
            //If User was never authenticated then we expect to get null
            if (userEmailAddress == null) return false;

            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Name == userEmailAddress && x.RoleStrength > 0);

            return user != null;
        }


        /// <summary>
        /// Returns a user by email address
        /// </summary>
        /// <param name="userEmailAddress">string?</param>
        /// <returns>UserModel</returns>
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
        /// <summary>
        /// Returns a user by Name 
        /// </summary>
        /// <param name="name">string?</param>
        /// <returns>UserModel</returns>
        public async Task<UserModel> GetUserByLoginNameAsync(string? name)
        {
            if (string.IsNullOrEmpty(name?.Trim()))
                throw new ArgumentNullException("User Name cannot be null");
            var user = await _context.Users
                .Include(x => x.Team)
                .FirstOrDefaultAsync(x => x.Name == name);

            if(user == null)
            {
                throw new NotFoundException("User", name);
            }

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
        /// <summary>
        /// Returns a user by UserId 
        /// </summary>
        /// <param name="userId">Int</param>
        /// <returns>UserModel</returns>
        public async Task<UserModel> GetUserByIdAsync(int userId)
        {
            var user = await _context.Users
                .Include(x => x.Team)
                .FirstOrDefaultAsync(x => x.UserID == userId);
            if (user == null)
            {
                throw new NotFoundException("User", userId.ToString());
            }
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
        /// <summary>
        /// Updates a User
        /// </summary>
        /// <param name="model">UserModel</param>
        public async Task UpdateUserAsync(UserModel model)
        {
            var user = await _context.Users.Include(x => x.Team).FirstOrDefaultAsync(x => x.UserID == model.UserId);
            if(user == null)
            {
                throw new NotFoundException("User", model.UserId.ToString());
            }

            user.Name = model.Name!;
            user.DisplayName = model.DisplayName!;
            user.email = model.Email;
            user.RoleStrength = model.RoleStrength;
            user.LastActive = model.LastActive;
            user.TeamID = model.TeamId;

            await _context.SaveChangesAsync();
        }
    }
}
