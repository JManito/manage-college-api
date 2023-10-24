using ManageCollege.Data;
using ManageCollege.Repositories.Interface;
using ManageCollege.Repositories.Implementation;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Injecting DBContext so we can communicate with DB from the controller or repo
builder.Services.AddDbContext<ApplicationDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ManageCollegeConnectionSting"));
});

builder.Services.AddScoped<IRepository, Repository>();

var app = builder.Build();

// Configure swagger if Dev
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Magnifinance - Manage College V1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();


//Allow ALL connections hack
app.UseCors(options =>
{
    options.AllowAnyHeader();
    options.AllowAnyOrigin();
    options.AllowAnyMethod();

});
 
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
 
app.Run();
