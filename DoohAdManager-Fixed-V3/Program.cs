using DoohAdManager.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add HttpContextAccessor
builder.Services.AddHttpContextAccessor();

// Add services with JSON options to handle circular references
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.WriteIndented = true;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() 
    { 
        Title = "DOOH Ad Manager API", 
        Version = "v1",
        Description = "Digital Out-of-Home Advertisement Management System"
    });
});

// Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection")));

// CORS - IMPORTANT: Must be before building the app
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Ensure wwwroot/uploads directory exists
var uploadsPath = Path.Combine(builder.Environment.WebRootPath, "uploads");
if (!Directory.Exists(uploadsPath))
{
    Directory.CreateDirectory(uploadsPath);
}

Console.WriteLine("===========================================");
Console.WriteLine($"WebRoot: {builder.Environment.WebRootPath}");
Console.WriteLine($"Uploads: {uploadsPath}");
Console.WriteLine($"Exists: {Directory.Exists(uploadsPath)}");
Console.WriteLine("===========================================");

// Use default static files middleware (serves from wwwroot)
app.UseStaticFiles();

Console.WriteLine("Static files enabled (serving from wwwroot)");

// Create database on startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    try
    {
        db.Database.EnsureCreated();
        Console.WriteLine("Database ensured created");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Database error: {ex.Message}");
    }
}

// Middleware - ORDER MATTERS!
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DOOH API v1"));
}

// CORS must come before Authorization and MapControllers
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();
app.UseMiddleware<ErrorHandlingMiddleware>();

Console.WriteLine("===========================================");
Console.WriteLine("Application started successfully!");
Console.WriteLine($"Test static files at: http://localhost:5085/uploads/test.txt");
Console.WriteLine("===========================================");

app.Run();
