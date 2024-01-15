using CrudExample.Controllers;
using ServiceContracts;
using Services;
using Microsoft.EntityFrameworkCore;
using Entities;

var builder = WebApplication.CreateBuilder(args);
//add services into ioc container
//builder.Services.AddSingleton<ICountriesService, CountriesService>();
//builder.Services.AddSingleton<IPersonsService, PersonsService>();
builder.Services.AddScoped<ICountriesService, CountriesService>();
builder.Services.AddScoped<IPersonsService, PersonsService>();
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<PersonDBContext>(
    options =>
    {
        options.UseSqlServer(builder
            .Configuration.GetConnectionString("DefaultConnectionString")); // for using sql server or any other sql
    });

//Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=PersonsDatabase;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False

var app = builder.Build();



//app.MapGet("/", () => "Hello World!");

if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

Rotativa.AspNetCore.RotativaConfiguration.Setup("wwwroot", wkhtmltopdfRelativePath: "Rotativa");  // for loading the exe file at run tiem and converting html content  to pdf.

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();
app.Run();
