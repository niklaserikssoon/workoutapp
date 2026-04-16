using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using User_API.Data;
using User_API.Filters;
using User_API.Models;
using User_API.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    // Custom Action Filters
    options.Filters.Add<ValidateModelFilter>();
    options.Filters.Add<PerformanceFilter>();
});

builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddHttpClient("WorkoutApi", client =>
{
    var baseUrl = builder.Configuration["ServiceUrls:WorkoutApi"];
    client.BaseAddress = new Uri(baseUrl!);
});

builder.Services.AddDbContext<UserDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("WorkoutDb")));

// Configure the HTTP request pipeline.
builder.Services.AddOpenApi("v1");

builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
})
.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

// CORS-Policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .WithOrigins("http://localhost:5501", "http://127.0.0.1:5501") //Frontend port
            .WithMethods("GET", "POST", "PUT", "DELETE")
            .WithHeaders("Authorization", "Content-Type");
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.Title = "User API";
        options.Theme = ScalarTheme.DeepSpace;
    });
}

app.UseHttpsRedirection();
app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();