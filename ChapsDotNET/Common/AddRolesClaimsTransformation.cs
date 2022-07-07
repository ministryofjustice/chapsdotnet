using ChapsDotNET.Business.Interfaces;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace ChapsDotNET.Common
{
    public class AddRolesClaimsTransformation : IClaimsTransformation
    {
        private readonly IUserComponent _userComponent;

        public AddRolesClaimsTransformation(IUserComponent userComponent)
        {
            _userComponent = userComponent;
        }

        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            // Clone current identity
            var clone = principal.Clone();
            var newIdentity = clone.Identity as ClaimsIdentity;

            // Support AD and local accounts
            var name = principal.Identity?.Name;
            if (name == null)
            {
                return principal;
            }

            // Get user from database
            var user = await _userComponent.GetUserByNameAsync(name);

            // Add role claims to cloned identity
            var claim = new Claim("RoleStrength", user.RoleStrength.ToString());
            newIdentity?.AddClaim(claim);

            return clone;
        }
    }
}
