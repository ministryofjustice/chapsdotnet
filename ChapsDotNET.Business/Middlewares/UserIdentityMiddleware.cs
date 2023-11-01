using System.Security.Claims;
using ChapsDotNET.Business.Interfaces;
using ChapsDotNET.Data.Contexts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace ChapsDotNET.Business.Middlewares
{
    public class UserIdentityMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly IServiceScopeFactory _serviceScopeFactory;

		public UserIdentityMiddleware(RequestDelegate next, IServiceScopeFactory serviceScopeFactory)
		{
			_next = next;
			_serviceScopeFactory = serviceScopeFactory;
		}

		public async Task InvokeAsync(HttpContext httpContext)
		{
			using var scope = _serviceScopeFactory.CreateScope();
			var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();

			int? userId = null;
			if (httpContext.User!.Identity!.IsAuthenticated)
			{
				var email = httpContext.User.FindFirstValue("preferred_username");

				if (!string.IsNullOrWhiteSpace(email))
				{
					var userComp = scope.ServiceProvider.GetRequiredService<IUserComponent>();
					var user = await userComp.GetUserByNameAsync(email);

					if (user != null)
					{
						userId = user.UserId;

						if (httpContext.User.Identity is ClaimsIdentity claimsIdentity && userId != null)
						{
							if (!claimsIdentity.HasClaim(c => c.Type == "userId"))
							{
								claimsIdentity.AddClaim(new Claim("userId", userId.ToString() ?? ""));
							}
						}
					}
				}
			}
			await _next(httpContext);
		}
	}
}