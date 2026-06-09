using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ChapsDotNET.Business.Middlewares
{
    /// <summary>
    /// Middleware to handle maintenance mode for the application.
    /// When enabled, all requests (except health checks) will be redirected to the maintenance page.
    /// </summary>
    public class MaintenanceModeMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<MaintenanceModeMiddleware> _logger;

        public MaintenanceModeMiddleware(RequestDelegate next, ILogger<MaintenanceModeMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, IConfiguration configuration)
        {
            // Check if maintenance mode is enabled
            var maintenanceModeEnabled = configuration.GetValue<bool>("MaintenanceMode:Enabled");

            if (!maintenanceModeEnabled)
            {
                await _next(context);
                return;
            }

            // Allow health check endpoints to continue
            if (context.Request.Path.StartsWithSegments("/dotnet-health"))
            {
                await _next(context);
                return;
            }

            // Serve the maintenance page
            _logger.LogInformation("Maintenance mode active - serving maintenance page");
            
            context.Response.StatusCode = 503;
            context.Response.Headers["Retry-After"] = "3600"; // Suggest retry after 1 hour
            context.Response.ContentType = "text/html";

            var maintenancePagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "maintenance.html");
            
            if (File.Exists(maintenancePagePath))
            {
                var content = await File.ReadAllTextAsync(maintenancePagePath);
                await context.Response.WriteAsync(content);
            }
            else
            {
                // Fallback simple HTML if maintenance.html is not found
                await context.Response.WriteAsync(@"
<!DOCTYPE html>
<html>
<head>
    <title>Service Unavailable</title>
    <meta name='robots' content='noindex, nofollow'>
</head>
<body>
    <h1>Service Unavailable</h1>
    <p>We are currently performing system upgrades or maintenance.</p>
    <p>Please try again later.</p>
</body>
</html>");
            }
        }
    }
}
