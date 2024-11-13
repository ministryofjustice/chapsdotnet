using System.Net;
using ChapsDotNET.Business.Components;
using ChapsDotNET.Business.Interfaces;
using ChapsDotNET.Business.Middlewares;
using ChapsDotNET.Common;
using ChapsDotNET.Data.Contexts;
using ChapsDotNET.Policies.Handlers;
using ChapsDotNET.Policies.Requirements;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Yarp.ReverseProxy.Forwarder;

var options = new WebApplicationOptions
{
    // EnvironmentName = "Development"
};

var builder = WebApplication.CreateBuilder(options);
var chapsDns = Environment.GetEnvironmentVariable("CHAPS_DNS");

if (string.IsNullOrEmpty(chapsDns))
{
    throw new InvalidOperationException("CHAPS_DNS environment variable is not set.");
}

var hostEntry = await Dns.GetHostEntryAsync(chapsDns);
var destinationIp = hostEntry.AddressList.FirstOrDefault()?.ToString();
var destinationPrefix = $"http://{destinationIp}:80/";

Console.WriteLine($"CHAPS_DNS set: {chapsDns}. Destination prefix: {destinationPrefix}");

var reverseProxySection = builder.Configuration.GetSection("ReverseProxy");
reverseProxySection.GetSection("Clusters:framework_481_Cluster:Destinations:framework481_app:Address")
        .Value = destinationPrefix;
// debug connection to CHAPS:
try
{
    
    Console.WriteLine($"Resolved {chapsDns} to {string.Join(", ", hostEntry.AddressList.Select(a => a.ToString()))}");
}
catch (Exception ex)
{
    Console.WriteLine($"DNS Resolution failed for {chapsDns}: {ex.Message}");
}

try
{
    using var client = new HttpClient { Timeout = TimeSpan.FromSeconds(30) };
    var response = await client.GetAsync($"{destinationPrefix}");
    Console.WriteLine(response.IsSuccessStatusCode ? "Connected to CHAPS" : "Failed to connect to CHAPS");
}
catch (Exception ex)
{
    Console.WriteLine($"Initial connectivity test failed: {ex.Message}");
}

var loggerFactory = LoggerFactory.Create(logging =>
{
    logging.AddConsole();
});
var logger = loggerFactory.CreateLogger<Program>();

builder.Configuration.AddEnvironmentVariables();

if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Debug);
builder.Logging.AddFilter("Microsoft.Data.SqlClient", LogLevel.Debug);

// Add services to the container.
builder.Services.AddControllersWithViews();

var dbName = builder.Configuration["DB_NAME"];
var rdsHostName = builder.Configuration["RDS_HOSTNAME"];
var rdsPassword = builder.Configuration["RDS_PASSWORD"];
var rdsPort = builder.Configuration["RDS_PORT"] ?? "1433";
var rdsUserName = builder.Configuration["RDS_USERNAME"];

SqlConnectionStringBuilder myConnectionString;
if (!string.IsNullOrEmpty(dbName) &&
    !string.IsNullOrEmpty(rdsHostName) &&
    !string.IsNullOrEmpty(rdsPassword) &&
    !string.IsNullOrEmpty(rdsPort) &&
    !string.IsNullOrEmpty(rdsUserName))
{
    // production config
    myConnectionString = new SqlConnectionStringBuilder
    {
        DataSource = $"{rdsHostName},{rdsPort}",
        InitialCatalog = dbName,
        UserID = rdsUserName,
        Password = rdsPassword,
        TrustServerCertificate = true,
        MultipleActiveResultSets = true
    };
}
else
{
    // development config 
    myConnectionString = new SqlConnectionStringBuilder
    {
        DataSource = @"ALISTAIRCUR98CF\SQLEXPRESS",
        InitialCatalog = "chaps-dev",
        IntegratedSecurity = true,
        TrustServerCertificate = true
    };
}

// custom httpClient to force HTTP/1.1 for old CHAPS
var httpClient = new HttpMessageInvoker(new SocketsHttpHandler()
{
    AllowAutoRedirect = false,
    AutomaticDecompression = DecompressionMethods.None,
    UseCookies = false,
    SslOptions = new System.Net.Security.SslClientAuthenticationOptions
    {
        // RemoteCertificateValidationCallback =
        // (sender, cert, chain, sslPolicyErrors) => true // Only use this in development!
    }
});

var connectionString = myConnectionString.ToString();

// Add services to the container.
builder.Services.AddSingleton(new DatabaseSettings { ConnectionString = connectionString});
builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(options =>
    {
        options.ClientId = builder.Configuration["CLIENT_ID"];;
        options.TenantId = builder.Configuration["TenantId"];
        options.Instance = builder.Configuration["Instance"];
        options.Domain = builder.Configuration["Domain"];
        options.CallbackPath = builder.Configuration["CallbackPath"];
    });

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
}).AddRazorRuntimeCompilation();

builder.Services.AddAuthorization(options =>
{
    // By default, all incoming requests will be authorized according to the default policy.
    options.FallbackPolicy = options.DefaultPolicy;
    options.AddPolicy("IsAuthorisedUser", isAuthorizedUserPolicy =>
    {
        isAuthorizedUserPolicy.Requirements.Add(new IsAuthorisedUserRequirement());
    });
});

builder.Services.AddDbContext<DataContext>(options => 
        options.UseSqlServer(myConnectionString.ConnectionString),
    ServiceLifetime.Scoped);
builder.Services.AddScoped<IAuthorizationHandler, IsAuthorisedUserHandler>();
builder.Services.AddScoped<ICampaignComponent, CampaignComponent>();
builder.Services.AddScoped<IClaimsTransformation, AddRolesClaimsTransformation>();
builder.Services.AddScoped<ILeadSubjectComponent, LeadSubjectComponent>();
builder.Services.AddScoped<IMoJMinisterComponent, MoJMinisterComponent>();
builder.Services.AddScoped<IMPComponent, MPComponent>();
builder.Services.AddScoped<IPublicHolidayComponent, PublicHolidayComponent>();
builder.Services.AddScoped<IReportComponent, ReportComponent>();
builder.Services.AddScoped<ISalutationComponent, SalutationComponent>();
builder.Services.AddScoped<ITeamComponent, TeamComponent>();
builder.Services.AddScoped<IUserComponent, UserComponent>();
builder.Services.AddScoped<IRoleComponent, RoleComponent>();
builder.Services.AddScoped<IAlertComponent, AlertComponent>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddReverseProxy()
    .LoadFromConfig(reverseProxySection);
builder.Services.AddHttpForwarder();
builder.Services.AddSingleton(httpClient);
builder.Services.AddHealthChecks();
builder.Services.AddAuthorizationBuilder().AddPolicy("HealthCheck", policy =>
{
    policy.RequireAssertion(context => true);
});

var app = builder.Build();


Console.WriteLine($"Current Env: {builder.Environment.EnvironmentName}");

var forwarder = app.Services.GetRequiredService<IHttpForwarder>();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// This code below, ending at line 'app.UseForwardedHeaders(forwardedHeaderOptions);' is required to redirect to https.
var forwardedHeaderOptions = new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
};
forwardedHeaderOptions.KnownNetworks.Clear();
forwardedHeaderOptions.KnownProxies.Clear();

app.UseForwardedHeaders(forwardedHeaderOptions);
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseMiddleware<UserIdentityMiddleware>();
app.UseAuthorization();

// configure proxy routes 
app.MapReverseProxy(proxyPipeline =>
{
    proxyPipeline.Run(async (context) =>
    {    
        var requestOptions = new ForwarderRequestConfig
        {
            Version = HttpVersion.Version11, // CHAPS requires we use http/1.1
            VersionPolicy = HttpVersionPolicy.RequestVersionExact // don't negotiate for a different version
        };
        try
        {
            var error = await forwarder.SendAsync(context, destinationPrefix, httpClient, requestOptions,
                HttpTransformer.Default,
                context.RequestAborted);
            if (error != ForwarderError.None)
            {
                Console.WriteLine($"Forwarding error: {error}");
                context.Response.StatusCode = 502;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception during forwarding: {ex.Message}");
            Console.WriteLine(ex.StackTrace);
            context.Response.StatusCode = 500;
        }
    });
});

app.MapAreaControllerRoute(
    name: "ChapsServices",
    areaName: "Admin",
    pattern: "Admin/{controller=Admin}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllers().RequireAuthorization("IsAuthorisedUser");
app.MapHealthChecks("/health").WithMetadata(new AllowAnonymousAttribute());

app.Run();
