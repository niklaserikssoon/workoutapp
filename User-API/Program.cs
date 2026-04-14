using User_API.Data;
using Microsoft.AspNetCore.Mvc;
using User_API.Models;
using Microsoft.EntityFrameworkCore;
using User_API.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddScoped<IUserService, UserService>();


builder.Services.AddHttpClient("WorkoutApi", client =>
{
    var baseUrl = builder.Configuration["ServiceUrls:WorkoutApi"];
    client.BaseAddress = new Uri(baseUrl!);
});

builder.Services.AddDbContext<UserDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
