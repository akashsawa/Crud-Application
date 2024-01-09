using CrudExample.Controllers;
using ServiceContracts;
using Services;

var builder = WebApplication.CreateBuilder(args);
//add services into ioc container
builder.Services.AddSingleton<ICountriesService, CountriesService>();
builder.Services.AddSingleton<IPersonsService, PersonsService>();
builder.Services.AddControllersWithViews();
var app = builder.Build();



//app.MapGet("/", () => "Hello World!");

if(builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
app.UseStaticFiles();
app.UseRouting();
app.MapControllers();
app.Run();
