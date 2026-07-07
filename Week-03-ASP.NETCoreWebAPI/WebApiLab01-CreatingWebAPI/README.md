# Lab 01 — Creating a Web API

## Objective

Build the first ASP.NET Core 8 Web API for the Retail Inventory System,
implementing GET and POST endpoints for Products with EF Core persistence.

---

## Concepts Covered

- What is a Web API and how it differs from MVC
- HTTP Methods: GET, POST, PUT, DELETE
- HTTP Status Codes: 200, 201, 400, 404, 500
- `[ApiController]` and `[Route]` attributes
- `ControllerBase` vs `Controller`
- `IActionResult` return types: `Ok()`, `NotFound()`, `CreatedAtAction()`, `BadRequest()`
- JSON serialization and circular reference handling
- `FromBody` — binding request body to a parameter
- ASP.NET Core Minimal Hosting Model (`Program.cs`)
- Middleware pipeline
- EF Core integration in Web API (constructor injection pattern)
- `EnsureCreated()` for automatic DB setup

---

## Folder Structure

```
WebApiLab01-CreatingWebAPI/
├── Controllers/
│   └── ProductsController.cs   — GET all, GET by ID, POST
├── Models/
│   ├── Category.cs
│   ├── Supplier.cs
│   └── Product.cs
├── Data/
│   └── ApplicationDbContext.cs — DbContext with seed data
├── Program.cs                  — Service registration & middleware pipeline
├── appsettings.json            — Connection string
├── WebApiLab01.csproj
└── README.md
```

---

## Prerequisites

- .NET 8 SDK
- SQL Server LocalDB

---

## How to Run

```bash
cd WebApiLab01-CreatingWebAPI
dotnet restore
dotnet run
```

Navigate to: `https://localhost:{port}/swagger`

---

## API Endpoints

| Method | Route | Description |
|--------|-------|-------------|
| GET | `/api/products` | Get all products |
| GET | `/api/products/{id}` | Get product by ID |
| POST | `/api/products` | Create a new product |

### POST Request Body Example

```json
{
  "name": "Bluetooth Speaker",
  "price": 49.99,
  "stockQuantity": 80,
  "categoryId": 1,
  "supplierId": 1
}
```

---

## Expected Output

```
GET /api/products → 200 OK
[
  {
    "id": 1,
    "name": "Laptop Pro 15",
    "price": 1299.99,
    "stockQuantity": 50,
    "categoryId": 1,
    "category": { "id": 1, "name": "Electronics", ... },
    ...
  },
  ...
]

GET /api/products/999 → 404 Not Found
{ "message": "Product with Id=999 was not found." }

POST /api/products → 201 Created
Location: /api/products/6
```

---

## Learning Outcomes

- Create an ASP.NET Core Web API project from scratch
- Define controllers with `[ApiController]` and `[Route]`
- Implement GET and POST endpoints
- Return appropriate HTTP status codes
- Integrate EF Core with constructor injection
- Understand the ASP.NET Core middleware pipeline
