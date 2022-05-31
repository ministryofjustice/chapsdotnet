using System.Data.SqlClient;
using ChapsDotNET.Contexts;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
var dbName = builder.Configuration["DB_NAME"];
var rdsHostName = builder.Configuration["RDS_HOSTNAME"];
var rdsPassword = builder.Configuration["RDS_PASSWORD"];
var rdsPort = builder.Configuration["RDS_PORT"];
var rdsUserName = builder.Configuration["RDS_USERNAME"];

var myConnectionString = new SqlConnectionStringBuilder();
myConnectionString.InitialCatalog = dbName;
myConnectionString.DataSource = $"{rdsHostName}, {rdsPort}";
myConnectionString.Password = rdsPassword;
myConnectionString.UserID = rdsUserName;
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(myConnectionString.ConnectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
