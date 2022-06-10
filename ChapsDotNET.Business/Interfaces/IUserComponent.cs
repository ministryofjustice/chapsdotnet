namespace ChapsDotNET.Business.Interfaces
{
    public interface IUserComponent
	{
		public int GetUsersCount();
        public Task<bool> IsUserAuthorised(string? userEmailAddress);
	}


}

