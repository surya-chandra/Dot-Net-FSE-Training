# Lab 01 — Understanding ORM & Entity Framework Core 8

## Objective

Understand what an ORM is, explore the EF Core 8 architecture, and set up
the foundational entity classes and `DbContext` for the Retail Inventory System.

---

## Concepts Covered

- What is an ORM (Object-Relational Mapper)?
- Benefits of EF Core over raw ADO.NET
- EF Core Architecture: Entity → DbContext → Provider → Database
- Entity Classes and their properties
- DbContext and DbSet<T>
- Code First Workflow overview
- Navigation Properties and Foreign Keys
- Fluent API configuration basics

---

## Folder Structure

```
EFCoreLab01-UnderstandingORM/
├── Models/
│   ├── Category.cs       — Category entity
│   ├── Supplier.cs       — Supplier entity
│   └── Product.cs        — Product entity (links Category & Supplier)
├── Data/
│   └── ApplicationDbContext.cs  — EF Core DbContext
├── Program.cs            — Demo: architecture inspection & entity creation
├── EFCoreLab01.csproj    — Project file with EF Core 8 packages
└── README.md
```

---

## Prerequisites

- .NET 8 SDK
- Visual Studio 2022 or VS Code
- SQL Server / LocalDB (optional for this lab — no DB connection required)

---

## How to Run

```bash
cd EFCoreLab01-UnderstandingORM
dotnet restore
dotnet run
```

> This lab does **not** connect to a database. It demonstrates EF Core's
> model inspection API to show entity types, columns, and relationships.

---

## Expected Output

```
==============================================
  Lab 01 — Understanding ORM & EF Core 8
==============================================

EF Core Architecture Overview
------------------------------
DbContext Type  : ApplicationDbContext
Entity Types registered in this context:
  • Category  →  Table: [Categories]
      Column: Id                   Type: int
      Column: Name                 Type: nvarchar(100)
      Column: Description          Type: nvarchar(500)
  • Supplier  →  Table: [Suppliers]
      ...
  • Product   →  Table: [Products]
      ...

Relationships:
--------------
  Product → Category  (FK: CategoryId)
  Product → Supplier  (FK: SupplierId)

Sample Entity Objects (in-memory, no DB connection):
------------------------------------------------------
  Category : Electronics — Electronic gadgets and devices
  Supplier : TechSupply Co. (contact@techsupply.com)
  Product  : Laptop Pro 15  |  Price: $1299.99  |  Stock: 50
             Category → Electronics
             Supplier → TechSupply Co.

Code First Workflow Steps:
--------------------------
  1. Define entity classes  (Models folder)
  2. Create ApplicationDbContext  (Data folder)
  3. dotnet ef migrations add InitialCreate
  4. dotnet ef database update
  5. Database & tables are created automatically!

Lab 01 complete. Proceed to Lab 02 — Database Context.
```

---

## Learning Outcomes

- Explain the difference between ADO.NET and EF Core
- Describe the EF Core Code First workflow
- Create entity classes with proper navigation properties
- Configure a `DbContext` using Fluent API
- Understand how EF Core maps C# classes to database tables
