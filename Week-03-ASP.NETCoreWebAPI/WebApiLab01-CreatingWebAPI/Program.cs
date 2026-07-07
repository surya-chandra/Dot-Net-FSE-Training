// ============================================================
//  Lab 01 — Creating a Web API
//  ASP.NET Core 8 Web API — Retail Inventory System
// ============================================================
//
//  PROGRAM.CS STRUCTURE (Minimal Hosting Model):
//  -----------------------------------------------
//  1. Create WebApplicationBuilder
//  2. Register services into the DI container  (builder.Services)
//  3. Build the app                            (builder.Build())
//  4. Configure the HTTP request pipeline      (app.Use...)
//  5. Run the app                              (app.Run())
//
//  The HTTP pipeline is a chain of middleware components.
//  Each middleware can process the request, pass it on,
//  and process the response on the way back.

using Microsoft.EntityFrameworkCore;
using WebApiLab01.Data;

var builder = WebApplication.CreateBuilder(args);

// ----------------------------------------------------------
// 1. Register Services
// ----------------------------------------------------------

// AddControllers() registers MVC controller support.
// This enables [ApiController] and [Route] attributes.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Prevent circular reference errors when serializing
        // entities with navigation properties (e.g. Product → Category → Products)
        options.JsonSerializerOptions.ReferenceHandler =
            System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

// Register EF Core with SQL Server.
// The connection string is read from appsettings.json.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// AddEndpointsApiExplorer() + AddSwaggerGen() enable Swagger UI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ----------------------------------------------------------
// 2. Ensure database is created and seeded
// ----------------------------------------------------------
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.EnsureCreated();
}

// ----------------------------------------------------------
// 3. Configure HTTP Request Pipeline (Middleware)
// ----------------------------------------------------------

// Show Swagger UI in development mode
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Redirect HTTP → HTTPS
app.UseHttpsRedirection();

// Route requests to the correct controller action
app.UseAuthorization();
app.MapControllers();

app.Run();
