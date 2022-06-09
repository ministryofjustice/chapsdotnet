using System;
namespace ChapsDotNET.Business.Interfaces
{
	public interface IUserComponent
	{
		public int GetUsersCount();
        public bool IsUserAuthorised(string? userEmailAddress);
	}


}

