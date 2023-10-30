using System;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.Extensions.DependencyInjection;
using ChapsDotNET.Business.Interfaces;
using Microsoft.Data.SqlClient;

namespace ChapsDotNET.Business.Middlewares
{
	public class UserIdentityMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly IServiceScopeFactory _serviceScopeFactory;
		private readonly string? _connectionString;

		public UserIdentityMiddleware(RequestDelegate next, IServiceScopeFactory serviceScopeFactory, DatabaseSettings databaseSettings)
		{
			_next = next;
			_serviceScopeFactory = serviceScopeFactory;
			_connectionString = databaseSettings.ConnectionString;
		}

		public async Task Invoke(HttpContext httpContext)		{
			int? userId = null;
			if(httpContext.User!.Identity!.IsAuthenticated)
			{
				var email = httpContext.User.FindFirstValue("preferred_username");
				var roleStrength = httpContext.User.FindFirstValue("rolestrength");

				if(!string.IsNullOrWhiteSpace(email))
				{
					using var scope = _serviceScopeFactory.CreateScope();
					{
						var userComp = scope.ServiceProvider.GetRequiredService<IUserComponent>();
						var user = await userComp.GetUserByNameAsync(email);

						if(user != null)
						{ 
							userId = user.UserId;

                            if (httpContext.User.Identity is ClaimsIdentity claimsIdentity && userId != null)
                            {
                                claimsIdentity.AddClaim(new Claim("userId", userId.ToString() ?? ""));
                            }
                        }
					}
				}
            }

			//set userdId in db session_context
			using var connection = new SqlConnection(_connectionString);
			connection.Open();
			using var cmd = connection.CreateCommand();
			cmd.CommandText = "EXEC sp_set_session_context 'UserID', @UserId";
			cmd.Parameters.Add(new SqlParameter("@UserId", userId));
			cmd.ExecuteNonQuery();
			//debug - check db returns userId
			using var comd = connection.CreateCommand();
			comd.CommandText = "SELECT SESSION_CONTEXT(N'UserID') AS UserId";
			var result = comd.ExecuteScalar().ToString();


            await _next(httpContext);
		}
	}
}

