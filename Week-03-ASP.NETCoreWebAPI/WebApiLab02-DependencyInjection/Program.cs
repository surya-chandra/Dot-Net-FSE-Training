// ============================================================
//  Lab 02 — Dependency Injection
//  ASP.NET Core 8 Web API — Retail Inventory System
// ============================================================
//
//  DEPENDENCY INJECTION (DI) LIFETIMES:
//  --------------------------------------
//  AddSingleton<T>()  — one instance for the entire app lifetime
//  AddScoped<T>()     — one instance per HTTP request  ← use for repos/services
//  AddTransient<T>()  — new instance every time it is requested
//
//  For repositories and services, AddScoped is the correct choice
//  because a single HTTP request should share the same DbContext
//  and repository instance throughout its lifetime.

using Microsoft.EntityFrameworkCore;
using WebApiLab02.Data;
using WebApiLab02.Interfaces;
using WebApiLab02.Repositories;
using WebApiLab02.Services;

var builder = WebApplication.CreateBuilder(args);

// ----------------------------------------------------------
// Register Services — DI Container Configuration
// ----------------------------------------------------------

builder.Services.AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.ReferenceHandler =
            System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles);

// EF Core DbContext — Scoped by default
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repository registration:
// When IProductRepository is requested, provide ProductRepository
// AddScoped = one instance per HTTP request
builder.Services.AddScoped<IProductRepository, ProductRepository>();

// Service registration:
// When IProductService is requested, provide ProductService
builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ----------------------------------------------------------
// Ensure database is created and seeded
// ----------------------------------------------------------
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.EnsureCreated();
}

// ----------------------------------------------------------
// HTTP Request Pipeline
// ----------------------------------------------------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
