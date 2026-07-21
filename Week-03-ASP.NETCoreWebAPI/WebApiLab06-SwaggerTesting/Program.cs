






















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

app.UseMiddleware<GlobalExceptionMiddleware>();




app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Retail Inventory API v1");
    options.RoutePrefix = "swagger";   
    options.DocumentTitle = "Retail Inventory API — Lab 06";
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
