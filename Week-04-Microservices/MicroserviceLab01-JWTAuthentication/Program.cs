// ============================================================
//  JWT Authentication Microservice
//  Cognizant GenC FSE Deep Skilling Program — Week 4
// ============================================================
//
//  MICROSERVICE ARCHITECTURE:
//  --------------------------
//  A microservice is a small, independently deployable service
//  that owns a single business capability. This service owns:
//    - User registration and authentication
//    - JWT token issuance and validation
//    - Role-based access control
//
//  AUTHENTICATION vs AUTHORISATION:
//  ---------------------------------
//  Authentication — "Who are you?"  (login, token validation)
//  Authorisation  — "What can you do?" (role checks, policies)
//
//  MIDDLEWARE PIPELINE ORDER (matters!):
//  --------------------------------------
//  1. GlobalExceptionMiddleware  — catches all errors
//  2. UseHttpsRedirection        — redirect HTTP → HTTPS
//  3. UseAuthentication          — validate JWT token
//  4. UseAuthorization           — check roles/policies
//  5. MapControllers             — route to controller actions

using System.Reflection;
using System.Text;
using JWTAuthService.Configuration;
using JWTAuthService.Data;
using JWTAuthService.Interfaces;
using JWTAuthService.Middleware;
using JWTAuthService.Repositories;
using JWTAuthService.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// ----------------------------------------------------------
// 1. Bind JwtSettings from appsettings.json
// ----------------------------------------------------------
builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("JwtSettings"));

var jwtSettings = builder.Configuration
    .GetSection("JwtSettings")
    .Get<JwtSettings>()!;

// ----------------------------------------------------------
// 2. Register EF Core with SQL Server
// ----------------------------------------------------------
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ----------------------------------------------------------
// 3. Register application services (DI container)
//    AddScoped = one instance per HTTP request
// ----------------------------------------------------------
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthService,    AuthService>();
builder.Services.AddScoped<IJwtService,     JwtService>();

// ----------------------------------------------------------
// 4. Configure JWT Bearer Authentication
//    Tells ASP.NET Core HOW to validate incoming tokens.
// ----------------------------------------------------------
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme    = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey         = new SymmetricSecurityKey(
                                       Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
        ValidateIssuer           = true,
        ValidIssuer              = jwtSettings.Issuer,
        ValidateAudience         = true,
        ValidAudience            = jwtSettings.Audience,
        ValidateLifetime         = true,
        ClockSkew                = TimeSpan.Zero
    };

    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            var logger = context.HttpContext.RequestServices
                .GetRequiredService<ILogger<Program>>();
            logger.LogWarning("JWT authentication failed: {Error}",
                context.Exception.Message);
            return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            var logger = context.HttpContext.RequestServices
                .GetRequiredService<ILogger<Program>>();
            logger.LogDebug("JWT token validated for: {User}",
                context.Principal?.Identity?.Name);
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization();

// ----------------------------------------------------------
// 5. Configure Controllers
// ----------------------------------------------------------
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy =
            System.Text.Json.JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.ReferenceHandler =
            System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

// ----------------------------------------------------------
// 6. Configure Swagger with JWT Bearer support
//    Adds the "Authorize" button so protected endpoints can
//    be tested directly from Swagger UI.
// ----------------------------------------------------------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title       = "JWT Authentication Microservice",
        Version     = "v1",
        Description = "An enterprise-ready JWT Authentication Microservice built with " +
                      "ASP.NET Core 8, EF Core 8, and SQL Server. " +
                      "Demonstrates Register, Login, JWT token generation, " +
                      "and role-based authorization (Admin / User). " +
                      "Part of the Cognizant GenC FSE Deep Skilling Program — Week 4.",
        Contact = new OpenApiContact
        {
            Name  = "Cognizant GenC Training",
            Email = "training@cognizant.com"
        }
    });

    // Security definition — enables the Authorize button in Swagger UI
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name         = "Authorization",
        Type         = SecuritySchemeType.Http,
        Scheme       = "Bearer",
        BearerFormat = "JWT",
        In           = ParameterLocation.Header,
        Description  = "Enter your JWT token.\n\nExample: eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
    });

    // Apply security globally — all endpoints show the lock icon
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id   = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });

    // Include XML documentation comments
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
        options.IncludeXmlComments(xmlPath);
});

// ----------------------------------------------------------
// 7. Build the application
// ----------------------------------------------------------
var app = builder.Build();

// ----------------------------------------------------------
// 8. Ensure database is created
// ----------------------------------------------------------
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.EnsureCreated();
}

// ----------------------------------------------------------
// 9. Configure HTTP Request Pipeline
//    ORDER IS CRITICAL
// ----------------------------------------------------------

// Must be first — wraps entire pipeline
app.UseMiddleware<GlobalExceptionMiddleware>();

// Swagger UI
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "JWT Auth Microservice v1");
    options.RoutePrefix   = "swagger";
    options.DocumentTitle = "JWT Authentication Microservice — Week 4";
});

app.UseHttpsRedirection();

// Authentication BEFORE Authorization — order matters
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
