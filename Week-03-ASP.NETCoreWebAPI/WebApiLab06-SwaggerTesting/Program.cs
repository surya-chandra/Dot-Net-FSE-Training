// ============================================================
//  Lab 06 — Swagger & API Testing
//  ASP.NET Core 8 Web API — Retail Inventory System
// ============================================================
//
//  SWAGGER / OPENAPI:
//  ------------------
//  Swagger is a toolset for documenting and testing REST APIs.
//  It generates an interactive UI from your code automatically.
//
//  COMPONENTS:
//  -----------
//  Swashbuckle.AspNetCore  — NuGet package that integrates Swagger
//  OpenAPI Specification   — JSON/YAML document describing the API
//  Swagger UI              — Browser-based interactive test client
//
//  HOW IT WORKS:
//  -------------
//  1. AddSwaggerGen() scans your controllers and generates
//     an OpenAPI JSON document at /swagger/v1/swagger.json
//  2. UseSwagger() serves that JSON document
//  3. UseSwaggerUI() serves the interactive HTML UI at /swagger

using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using WebApiLab06.Data;
using WebApiLab06.Interfaces;
using WebApiLab06.Middleware;
using WebApiLab06.Repositories;
using WebApiLab06.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.ReferenceHandler =
            System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddEndpointsApiExplorer();

// ----------------------------------------------------------
// Configure Swagger with full API metadata
// ----------------------------------------------------------
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title       = "Retail Inventory API",
        Version     = "v1",
        Description = "A RESTful API for managing the Retail Inventory System. " +
                      "Built with ASP.NET Core 8 and Entity Framework Core 8 " +
                      "as part of the Cognizant GenC FSE Deep Skilling Program — Week 3.",
        Contact = new OpenApiContact
        {
            Name  = "Cognizant GenC Training",
            Email = "training@cognizant.com"
        }
    });

    // Include XML comments from the project's documentation file
    // This populates Swagger UI with /// <summary> descriptions
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
        options.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.EnsureCreated();
}

// Global exception middleware — must be first
app.UseMiddleware<GlobalExceptionMiddleware>();

// ----------------------------------------------------------
// Enable Swagger in all environments for this training lab
// (In production, restrict to Development only)
// ----------------------------------------------------------
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Retail Inventory API v1");
    options.RoutePrefix = "swagger";   // UI available at /swagger
    options.DocumentTitle = "Retail Inventory API — Lab 06";
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
