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
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Yarp.ReverseProxy.Forwarder;

var builder = WebApplication.CreateBuilder();

var loggerFactory = LoggerFactory.Create(logging =>
{
    logging.AddConsole();
});

var chapsLocal = string.Empty;

builder.Configuration.AddEnvironmentVariables();
builder.Configuration.AddUserSecrets<Program>();

// foreach (DictionaryEntry envVar in Environment.GetEnvironmentVariables())
// {
//     Console.WriteLine($"EnvVar: {envVar.Key} = {envVar.Value}");
// }

if (builder.Environment.IsDevelopment())
{
    chapsLocal = "https://localhost:44300/";
}
else
{
    chapsLocal = "http://localhost:80/";
}

Console.WriteLine($"Current Env: {builder.Environment.EnvironmentName}, Chaps container address: {chapsLocal}");

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
        DataSource = rdsHostName,
        InitialCatalog = dbName,
        IntegratedSecurity = true,
        TrustServerCertificate = true
    };
}

// custom httpClient to force HTTP/1.1 for old CHAPS
var httpClient = new HttpMessageInvoker(new SocketsHttpHandler
{
    AllowAutoRedirect = false,
    AutomaticDecompression = DecompressionMethods.None,
    UseCookies = false,
    SslOptions = new System.Net.Security.SslClientAuthenticationOptions()
});

var connectionString = myConnectionString.ToString();

// Add services to the container.
builder.Services.AddSingleton(new DatabaseSettings { ConnectionString = connectionString});
builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(options =>
    {
        options.ClientId = builder.Configuration["CLIENT_ID"];
        options.TenantId = builder.Configuration["TenantId"];
        options.Instance = builder.Configuration["Instance"]!;
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
    options.AddPolicy("IsAuthorisedUser", isAuthorizedUserPolicy =>
    {
        isAuthorizedUserPolicy.Requirements.Add(new IsAuthorisedUserRequirement());
    });
});

builder.Services.AddDbContext<DataContext>(options => 
        options.UseSqlServer(myConnectionString.ConnectionString));
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
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));
builder.Services.AddHttpForwarder();
builder.Services.AddSingleton(httpClient);
builder.Services.AddHealthChecks();
builder.Services.AddAuthorizationBuilder().AddPolicy("HealthCheck", policy =>
{
    policy.RequireAssertion(context => true);
});

var app = builder.Build();
app.UseStaticFiles();
app.UseWhen(
    context => !context.Request.Path.StartsWithSegments("/dotnet-health"),
    b => b.UseAuthentication().UseAuthorization()
);
app.UseHealthChecks("/dotnet-health");
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

app.UseRouting();
app.UseAuthentication();

app.Use(async (context, next) =>
{
    if (context.User.Identity is { IsAuthenticated: false })
    {
        await context.ChallengeAsync(OpenIdConnectDefaults.AuthenticationScheme, new AuthenticationProperties
        {
            RedirectUri = context.Request.Path
        });
        return;
    }
    await next();
});

app.UseMiddleware<UserIdentityMiddleware>();
app.UseAuthorization();

// configure proxy routes 
app.MapReverseProxy(proxyPipeline =>
{
    var forwarder = app.Services.GetRequiredService<IHttpForwarder>();
    
    proxyPipeline.Run(async context =>
    {
        // Add the transformed headers to the proxy request
        if (context.User.Identity!.IsAuthenticated)
        {
            var userName = context.User.Identity.Name;
            var roleStrengthClaim = context.User.FindFirst("RoleStrength");
            
            context.Request.Headers.Append("X-User-Name", userName);
            //Console.WriteLine($"X-User-Name set: {userName}");
          
            if (roleStrengthClaim != null)
            {
                var roleStrength = roleStrengthClaim.Value;
                context.Request.Headers.Append("X-User-RoleStrength", roleStrength);
                //Console.WriteLine($"X-User-RoleStrength added: {roleStrength}");
            }
            
            await Task.CompletedTask;
        }
        
        // Console.WriteLine($"Forwarding request with headers: ");
        // foreach (var header in context.Request.Headers)
        // {
        //     Console.WriteLine($"{header.Key}: {string.Join(", ", header.Value)}");
        // }
      
        var requestOptions = new ForwarderRequestConfig
        {
            ActivityTimeout = TimeSpan.FromMinutes(10),
            Version = HttpVersion.Version11, // CHAPS requires we use http/1.1
            VersionPolicy = HttpVersionPolicy.RequestVersionExact // don't negotiate for a different version
        };
        
        Console.WriteLine($"Proxying request for: {context.Request.Path}");
        
        try
        {
            var error = await forwarder.SendAsync(context, chapsLocal, httpClient, requestOptions,
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

app.Run();
