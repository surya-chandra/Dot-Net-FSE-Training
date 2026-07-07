# Lab 02 — Dependency Injection

## Objective

Implement the Repository Pattern and Service Layer with ASP.NET Core's
built-in Dependency Injection container for the Retail Inventory System.

---

## Concepts Covered

- Inversion of Control (IoC) — who creates objects?
- Dependency Injection (DI) — injecting dependencies via constructor
- Repository Pattern — encapsulates data access behind an interface
- Service Layer — contains business logic, orchestrates repositories
- Interface-based programming — depend on abstractions, not concretions
- `AddScoped()` vs `AddSingleton()` vs `AddTransient()`
- Constructor injection in Controllers and Services
- SOLID: Dependency Inversion Principle (D in SOLID)

---

## Architecture

```
ProductsController
    ↓ depends on IProductService
ProductService
    ↓ depends on IProductRepository
ProductRepository
    ↓ depends on ApplicationDbContext
SQL Server
```

---

## Folder Structure

```
WebApiLab02-DependencyInjection/
├── Controllers/
│   └── ProductsController.cs
├── Models/
│   ├── Category.cs
│   ├── Supplier.cs
│   └── Product.cs
├── Data/
│   └── ApplicationDbContext.cs
├── Interfaces/
│   ├── IProductRepository.cs
│   └── IProductService.cs
├── Repositories/
│   └── ProductRepository.cs
├── Services/
│   └── ProductService.cs
├── Program.cs
├── appsettings.json
├── WebApiLab02.csproj
└── README.md
```

---

## How to Run

```bash
cd WebApiLab02-DependencyInjection
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
| PUT | `/api/products/{id}` | Update a product |
| DELETE | `/api/products/{id}` | Delete a product |

---

## Learning Outcomes

- Implement the Repository and Service patterns
- Register services with `AddScoped<TInterface, TImplementation>()`
- Understand DI lifetimes: Scoped, Singleton, Transient
- Apply the Dependency Inversion Principle
- Write loosely coupled, testable code
