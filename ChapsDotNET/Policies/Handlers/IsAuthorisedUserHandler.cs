using ChapsDotNET.Business.Interfaces;
using ChapsDotNET.Policies.Requirements;
using Microsoft.AspNetCore.Authorization;

namespace ChapsDotNET.Policies.Handlers
{
    public class IsAuthorisedUserHandler : AuthorizationHandler<IsAuthorisedUserRequirement>
	{
        private readonly IUserComponent _userComponent;

        public IsAuthorisedUserHandler(IUserComponent userComponent)
        {
            _userComponent = userComponent;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, IsAuthorisedUserRequirement requirement)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (requirement == null)
                throw new ArgumentNullException(nameof(requirement));
            
            var isChapsUser =  await _userComponent.IsUserAuthorisedAsync(context.User.Identity?.Name);
            if (isChapsUser)
            {
                context.Succeed(requirement);
            }
        }
    }
}

