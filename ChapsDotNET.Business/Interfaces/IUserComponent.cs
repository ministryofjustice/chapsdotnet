namespace ChapsDotNET.Business.Interfaces
{
    public interface IUserComponent
	{
        public Task<bool> IsUserAuthorisedAsync(string? userEmailAddress);
	}


}

