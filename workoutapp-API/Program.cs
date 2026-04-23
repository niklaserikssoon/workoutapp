using Asp.Versioning;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using workoutapp_API.Filters;
using workoutapp_API.services;
using Scalar.AspNetCore;
using WorkoutApp.API.Data;
using workoutapp_API.services.External;
using workoutapp_API.services.Exercises;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Database context
builder.Services.AddDbContext<WorkoutDbContext>(options =>
    options.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=WorkoutDb;Trusted_Connection=True;TrustServerCertificate=True;"));


// // OpenAPI with XML comments for documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFileName = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFileName));
});


//  cache
builder.Services.AddMemoryCache();

// External exercises (HTTP client)
builder.Services.AddHttpClient<IExternalExercise, ExternalExercise>(client =>
{
    client.BaseAddress = new Uri("https://raw.githubusercontent.com/yuhonas/free-exercise-db/main/");
});

// Local exercise service 
builder.Services.AddScoped<IExerciseService, ExerciseService>();


builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = 429;

    options.AddFixedWindowLimiter("writePolicy", limiterOptions =>
    {
        limiterOptions.PermitLimit = 3; // Allow 3 requests per window
        limiterOptions.Window = TimeSpan.FromSeconds(10); // 10-second window
        limiterOptions.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst; 
        limiterOptions.QueueLimit = 0; // No queuing, reject requests immediately if the limit is reached
    });
});

// API versioning
builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
});


// CORS-Policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .WithOrigins("http://localhost:5501", "http://127.0.0.1:5501")  //Frontend port
            .WithMethods("GET", "POST", "PUT", "DELETE")
            .WithHeaders("Authorization", "Content-Type");
    });
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

app.UseRateLimiter();

app.MapControllers();
app.MapScalarApiReference(options => 
{ 
    options.WithOpenApiRoutePattern("/swagger/{documentName}/swagger.json");
});


app.Run();