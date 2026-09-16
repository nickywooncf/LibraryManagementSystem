using Microsoft.EntityFrameworkCore;
using LibraryManagement.Data; // Ensure this matches your namespace
using LibraryManagement.Services; // Assuming you put ILibraryOperationsService here

var builder = WebApplication.CreateBuilder(args);

// ==========================================
// 1. Add services to the DI container.
// ==========================================

builder.Services.AddControllers();

// Configure Entity Framework Core with an In-Memory Database for testing
// (For production, swap "UseInMemoryDatabase" with "UseSqlServer(connectionString)")
builder.Services.AddDbContext<LibraryDbContext>(options =>
    options.UseInMemoryDatabase("LibraryDb"));

// Register Business Logic Services (Dependency Injection)
// AddScoped ensures a new instance is created per HTTP request
builder.Services.AddScoped<ILibraryOperationsService, LibraryOperationsService>();

// Configure Swagger/OpenAPI for testing the endpoints via a web UI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ==========================================
// 2. Build the app and configure the HTTP request pipeline.
// ==========================================
var app = builder.Build();

// Enable Swagger UI if in the Development environment
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Library API v1");
    });
}

// Redirect HTTP requests to HTTPS
app.UseHttpsRedirection();

// Enable authorization (even if not strictly used in this basic model, it's good practice to include it)
app.UseAuthorization();

// Map the controllers to the routing engine
app.MapControllers();

// Start the application
app.Run();