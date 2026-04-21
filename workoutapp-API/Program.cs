using Asp.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;
using WorkoutApp.API.Data;
using workoutapp_API.Filters;
using workoutapp_API.services;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    // Custom Action Filters
    options.Filters.Add<ValidateModelFilter>();
    options.Filters.Add<PerformanceFilter>();
});

builder.Services.AddDbContext<WorkoutDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("WorkoutDb")));

// Memory cache + typed HTTP client
builder.Services.AddMemoryCache();
builder.Services.AddHttpClient<IExternalExercise, ExerciseService>(client =>
{
    client.BaseAddress = new Uri("https://raw.githubusercontent.com/yuhonas/free-exercise-db/main/");
});

builder.Services.AddHttpClient("WorkoutApi", client =>
{
    var baseUrl = builder.Configuration["ServiceUrls:WorkoutApi"];
    client.BaseAddress = new Uri(baseUrl!);
});

// OpenAPI, single registration with JWT security definition
builder.Services.AddOpenApi("v1", options =>
{
    options.AddDocumentTransformer((doc, context, ct) =>
    {
        doc.Components ??= new();
        doc.Components.SecuritySchemes = new Dictionary<string, OpenApiSecurityScheme>
        {
            ["Bearer"] = new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Enter your JWT token"
            }
        };
        return Task.CompletedTask;
    });
});

// API versioning
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
            .WithOrigins("http://localhost:5501", "http://127.0.0.1:5501")  //Frontend port
            .WithMethods("GET", "POST", "PUT", "DELETE")
            .WithHeaders("Authorization", "Content-Type");
    });
});

// Rate Limiting
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("fixed", limiterOptions =>
    {
        limiterOptions.PermitLimit = 20;                                        // max 20 requests
        limiterOptions.Window = TimeSpan.FromSeconds(30);                       // per 30 seconds
        limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst; //
        limiterOptions.QueueLimit = 5;                                          // queue up to 5 extra requests
    });

    options.RejectionStatusCode = 429;                                          // HTTP 429 = Too Many Requests Status Code
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.Title = "Workout API";
        options.Theme = ScalarTheme.DeepSpace;
    });
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseRateLimiter();
app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers().RequireRateLimiting("fixed"); // Adds rate limiting globally to all controllers. (Instead of per controller or endpoint)

app.Run();