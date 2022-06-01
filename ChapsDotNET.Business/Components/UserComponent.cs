using System;
using ChapsDotNET.Business.Interfaces;
using ChapsDotNET.Data.Contexts;

namespace ChapsDotNET.Business.Components
{
	public class UserComponent : IUserComponent
	{
        private readonly DataContext context;

        public UserComponent(DataContext context)
		{
            this.context = context;
        }

        public int GetUsersCount()
        {
            return context.Users.Count();
        }
    }
}

