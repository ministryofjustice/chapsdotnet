using Microsoft.AspNetCore.Authorization;

namespace ChapsDotNET.Policies.Requirements
{
    public class IsAuthorisedUserRequirement : IAuthorizationRequirement
    {
        public IsAuthorisedUserRequirement()
        {
        }
    }
}

