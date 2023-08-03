using System.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MySqlConnector;
using SalesMVC.Data;
using Pomelo.EntityFrameworkCore.MySql;
using SalesMVC.Services;
using Microsoft.AspNetCore.Localization;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("SalesMVCContext");

string dbHost = Environment.GetEnvironmentVariable("databaseserverip");
string dbUsername = Environment.GetEnvironmentVariable("dbusername");
string dbPassword = Environment.GetEnvironmentVariable("dbuserpassword");


connectionString = String.Format(connectionString, dbHost, dbUsername, dbPassword);


var serverVersion = new MySqlServerVersion(new Version(8, 0, 31));

builder.Services.AddDbContext<SalesMVCContext>(
    options => options.UseMySql(
            connectionString, 
            serverVersion,
            builder => builder.MigrationsAssembly("SalesMVC")
            )
    );


// Add services to the container.
builder.Services.AddControllersWithViews();

// seeding service definido na pasta data

// registrar o servico de SeedingService
builder.Services.AddScoped<SeedingService>();
builder.Services.AddScoped<SellerService>();
builder.Services.AddScoped<DepartmentService>();
builder.Services.AddScoped<SalesRecordsService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    // call seeding service here
    // testar se ja existem dados na base

    using(var scope = app.Services.CreateScope())
    {
        var serviceScope = scope.ServiceProvider;

        var seedService = serviceScope.GetService<SeedingService>();

        seedService.Seed();
    }

}


var enUS = new CultureInfo("en-US");
var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(enUS),
    SupportedCultures = new List<CultureInfo> { enUS },
    SupportedUICultures = new List<CultureInfo> { enUS }
};

app.UseRequestLocalization(localizationOptions);

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
