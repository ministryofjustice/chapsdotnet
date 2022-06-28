namespace ChapsDotNET.Business.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string entityName, string Id) : base($"{entityName} with {Id} not found.")
        {

        }
    }
}
