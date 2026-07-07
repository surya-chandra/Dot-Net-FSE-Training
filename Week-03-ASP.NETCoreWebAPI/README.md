# Week 03 — ASP.NET Core Web API

## Week Overview

This week covers **ASP.NET Core Web API** — Microsoft's framework for building
RESTful HTTP services with .NET 8. All labs use the **Retail Inventory System**
domain with **Entity Framework Core 8** for persistence and build progressively
toward a fully documented, production-quality API.

---

## Lab Descriptions

| Lab | Title | Key Concepts |
|-----|-------|--------------|
| Lab 01 | Creating a Web API | Controllers, HTTP methods, JSON serialization, REST basics |
| Lab 02 | Dependency Injection | Repository pattern, service layer, IoC, AddScoped |
| Lab 03 | Routing & Controllers | Attribute routing, route/query params, full CRUD |
| Lab 04 | Model Validation | Data Annotations, ModelState, validation error responses |
| Lab 05 | Exception Handling | Global middleware, custom errors, logging, HTTP status codes |
| Lab 06 | Swagger & API Testing | Swagger/OpenAPI, XML docs, full CRUD testing via Swagger UI |

---

## API Architecture

```
HTTP Request
    ↓
Controller  (receives request, returns IActionResult)
    ↓
Service     (business logic, orchestration)
    ↓
Repository  (data access via EF Core)
    ↓
DbContext   (SQL Server)
```

Each layer depends only on the layer below it via interfaces — this is the
**Dependency Inversion Principle** in action.

---

## Required NuGet Packages

```xml
<PackageReference Include="Microsoft.EntityFrameworkCore" Version="8.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="8.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="8.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="8.0.0" />
<PackageReference Include="Swashbuckle.AspNetCore" Version="6.5.0" />
```

---

## Folder Structure

```
Week-03-ASP.NETCoreWebAPI/
├── WebApiLab01-CreatingWebAPI/
│   ├── Controllers/
│   ├── Models/
│   ├── Data/
│   ├── Program.cs
│   ├── appsettings.json
│   ├── WebApiLab01.csproj
│   └── README.md
├── WebApiLab02-DependencyInjection/
│   ├── Controllers/
│   ├── Models/
│   ├── Data/
│   ├── Interfaces/
│   ├── Repositories/
│   ├── Services/
│   ├── Program.cs
│   ├── appsettings.json
│   ├── WebApiLab02.csproj
│   └── README.md
├── WebApiLab03-RoutingAndControllers/
│   ├── Controllers/
│   ├── Models/
│   ├── DTOs/
│   ├── Data/
│   ├── Interfaces/
│   ├── Repositories/
│   ├── Services/
│   ├── Program.cs
│   ├── appsettings.json
│   ├── WebApiLab03.csproj
│   └── README.md
├── WebApiLab04-ModelValidation/
│   ├── Controllers/
│   ├── Models/
│   ├── DTOs/
│   ├── Data/
│   ├── Interfaces/
│   ├── Repositories/
│   ├── Services/
│   ├── Program.cs
│   ├── appsettings.json
│   ├── WebApiLab04.csproj
│   └── README.md
├── WebApiLab05-ExceptionHandling/
│   ├── Controllers/
│   ├── Models/
│   ├── DTOs/
│   ├── Data/
│   ├── Exceptions/
│   ├── Interfaces/
│   ├── Middleware/
│   ├── Repositories/
│   ├── Services/
│   ├── Program.cs
│   ├── appsettings.json
│   ├── WebApiLab05.csproj
│   └── README.md
├── WebApiLab06-SwaggerTesting/
│   ├── Controllers/
│   ├── Models/
│   ├── DTOs/
│   ├── Data/
│   ├── Exceptions/
│   ├── Interfaces/
│   ├── Middleware/
│   ├── Repositories/
│   ├── Services/
│   ├── Program.cs
│   ├── appsettings.json
│   ├── WebApiLab06.csproj
│   └── README.md
├── Screenshots/
└── README.md
```

---

## SQL Server Configuration

Each lab uses its own database. Update `appsettings.json` in each lab:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=RetailInventoryDb_Lab0X;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

> Replace `Lab0X` with the lab number (Lab01 through Lab06).

---

## Swagger Setup

Swagger UI is available at:

```
https://localhost:{port}/swagger
```

Run the project and navigate to the Swagger URL shown in the console.

---

## REST API Conventions Used

| HTTP Method | Route | Action |
|-------------|-------|--------|
| GET | `/api/products` | Get all products |
| GET | `/api/products/{id}` | Get product by ID |
| POST | `/api/products` | Create new product |
| PUT | `/api/products/{id}` | Update existing product |
| DELETE | `/api/products/{id}` | Delete product |

---

## Learning Outcomes

By the end of Week 3, you will be able to:

- Build a RESTful Web API with ASP.NET Core 8
- Apply the Repository and Service patterns with Dependency Injection
- Configure attribute routing and handle route/query parameters
- Validate incoming request models using Data Annotations
- Implement global exception handling middleware
- Document and test APIs using Swagger/OpenAPI
- Return correct HTTP status codes for all scenarios
- Integrate EF Core with a Web API project

---

## Prerequisites

- .NET 8 SDK
- Visual Studio 2022 or VS Code
- SQL Server LocalDB
- EF Core CLI: `dotnet tool install --global dotnet-ef`
- Postman (optional — Swagger UI is built in)
