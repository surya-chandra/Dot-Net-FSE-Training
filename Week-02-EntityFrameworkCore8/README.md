# Week 02 - Entity Framework Core 8

## Week Overview

This week covers **Entity Framework Core 8 (EF Core 8)** — Microsoft's modern,
lightweight, and extensible Object-Relational Mapper (ORM) for .NET applications.
All labs use a **Retail Inventory System** domain with a **Code First** approach
targeting **SQL Server**.

---

## Lab Descriptions

| Lab | Title | Key Concepts |
|-----|-------|--------------|
| Lab 01 | Understanding ORM | ORM theory, EF Core architecture, entity classes, DbContext |
| Lab 02 | Database Context | ApplicationDbContext, connection strings, DbSet configuration |
| Lab 03 | EF Core Migrations | Creating & applying migrations, schema generation |
| Lab 04 | Insert Data | Add(), AddRange(), SaveChanges(), seeding sample data |
| Lab 05 | Retrieve Data | Find(), First(), Where(), ToList(), OrderBy() |
| Lab 06 | Update & Delete | Update(), Remove(), SaveChanges(), exception handling |
| Lab 07 | LINQ Queries | Where, Select, GroupBy, aggregates, Include, ThenInclude |

---

## Domain Entities

All labs share the same **Retail Inventory System** domain:

- **Category** — product category (e.g., Electronics, Clothing)
- **Supplier** — product supplier with contact details
- **Product** — inventory item linked to a Category and Supplier

---

## Required NuGet Packages

```xml
<PackageReference Include="Microsoft.EntityFrameworkCore" Version="8.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="8.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="8.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="8.0.0" />
```

Install via CLI:

```bash
dotnet add package Microsoft.EntityFrameworkCore --version 8.0.0
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 8.0.0
dotnet add package Microsoft.EntityFrameworkCore.Tools --version 8.0.0
dotnet add package Microsoft.EntityFrameworkCore.Design --version 8.0.0
```

---

## EF Core CLI Commands

```bash
# Install EF Core CLI tools globally (once)
dotnet tool install --global dotnet-ef

# Create a new migration
dotnet ef migrations add InitialCreate

# Apply migrations to the database
dotnet ef database update

# List all migrations
dotnet ef migrations list

# Remove the last migration (if not applied)
dotnet ef migrations remove

# Drop the database
dotnet ef database drop
```

---

## SQL Server Setup

1. Install **SQL Server 2019/2022** or use **SQL Server Express** (free).
2. Install **SQL Server Management Studio (SSMS)** for visual management.
3. Ensure the SQL Server service is running.
4. Update the connection string in each lab's `ApplicationDbContext.cs`:

```csharp
optionsBuilder.UseSqlServer(
    "Server=(localdb)\\mssqllocaldb;Database=RetailInventoryDb;Trusted_Connection=True;"
);
```

> **Tip:** `(localdb)\mssqllocaldb` is available by default with Visual Studio 2022.

---

## Folder Structure

```
Week-02-EntityFrameworkCore8/
├── EFCoreLab01-UnderstandingORM/
│   ├── Models/
│   │   ├── Category.cs
│   │   ├── Supplier.cs
│   │   └── Product.cs
│   ├── Data/
│   │   └── ApplicationDbContext.cs
│   ├── Program.cs
│   ├── EFCoreLab01.csproj
│   └── README.md
├── EFCoreLab02-DatabaseContext/
│   ├── Models/
│   ├── Data/
│   ├── Program.cs
│   ├── EFCoreLab02.csproj
│   └── README.md
├── EFCoreLab03-Migrations/
│   ├── Models/
│   ├── Data/
│   ├── Migrations/
│   ├── Program.cs
│   ├── EFCoreLab03.csproj
│   └── README.md
├── EFCoreLab04-InsertData/
├── EFCoreLab05-RetrieveData/
├── EFCoreLab06-UpdateDelete/
├── EFCoreLab07-LINQQueries/
├── Screenshots/
└── README.md
```

---

## Learning Outcomes

By the end of Week 2, you will be able to:

- Explain what an ORM is and why EF Core is preferred over raw ADO.NET
- Create entity classes and configure relationships using EF Core conventions
- Set up `ApplicationDbContext` with SQL Server connection
- Generate and apply database migrations using the EF Core CLI
- Perform full CRUD operations using EF Core
- Write efficient LINQ queries including projections, grouping, and aggregates
- Use `Include()` and `ThenInclude()` for eager loading related data
- Handle exceptions gracefully in data access operations

---

## Prerequisites

- .NET 8 SDK
- Visual Studio 2022 (or VS Code with C# extension)
- SQL Server (LocalDB is sufficient)
- EF Core CLI tools (`dotnet tool install --global dotnet-ef`)
