using Asp.Versioning;
using workoutapp_API.services;
using WorkoutApp.API.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<WorkoutDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("WorkoutDb")));

// Add services to the container.
builder.Services.AddControllers();

// OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// HttpClient + cache
builder.Services.AddMemoryCache();
builder.Services.AddHttpClient<IExternalExercise, ExerciseService>(client =>
{
    client.BaseAddress = new Uri("https://raw.githubusercontent.com/yuhonas/free-exercise-db/main/");
});

// API versioning
builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();