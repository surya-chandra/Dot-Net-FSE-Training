# Lab 02 — Database Context

## Objective

Configure `ApplicationDbContext` with SQL Server, register all entity `DbSet`
properties, and apply Fluent API configuration for the Retail Inventory System.

---

## Concepts Covered

- `DbContext` class and its responsibilities
- `DbSet<T>` — mapping C# classes to database tables
- `OnConfiguring()` — setting up the SQL Server connection
- `OnModelCreating()` — Fluent API for schema rules
- Data Annotations vs Fluent API
- Unique indexes and default values
- `CanConnect()` — testing database connectivity
- `DbContext` lifetime and disposal

---

## Folder Structure

```
EFCoreLab02-DatabaseContext/
├── Models/
│   ├── Category.cs       — Data Annotations example
│   ├── Supplier.cs       — Data Annotations example
│   └── Product.cs        — Foreign key configuration
├── Data/
│   └── ApplicationDbContext.cs  — Full DbContext with Fluent API
├── Program.cs            — Context inspection & connectivity test
├── EFCoreLab02.csproj
└── README.md
```

---

## Prerequisites

- .NET 8 SDK
- SQL Server LocalDB (installed with Visual Studio 2022)
- EF Core CLI: `dotnet tool install --global dotnet-ef`

---

## How to Run

```bash
cd EFCoreLab02-DatabaseContext
dotnet restore
dotnet run
```

---

## Expected Output

```
==============================================
  Lab 02 — Database Context
==============================================

DbContext Information:
----------------------
  Context Type     : ApplicationDbContext
  Database Name    : RetailInventoryDb_Lab02
  Provider         : Microsoft.EntityFrameworkCore.SqlServer

Registered DbSets (Tables):
----------------------------
  DbSet<Category>   →  [Categories]  (3 columns)
  DbSet<Supplier>   →  [Suppliers]   (4 columns)
  DbSet<Product>    →  [Products]    (6 columns)

...

Database Connectivity Test:
---------------------------
  ✓ Successfully connected to SQL Server.
  ✓ Run 'dotnet ef database update' to create tables.
```

---

## Learning Outcomes

- Configure `ApplicationDbContext` for SQL Server
- Register entities as `DbSet<T>` properties
- Apply Fluent API configuration for constraints and relationships
- Test database connectivity programmatically
- Understand the difference between Data Annotations and Fluent API
