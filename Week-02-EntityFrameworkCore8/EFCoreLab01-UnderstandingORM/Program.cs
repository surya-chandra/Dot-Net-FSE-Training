using EFCoreLab01.Data;
using EFCoreLab01.Models;

// ============================================================
//  Lab 01 — Understanding ORM & Entity Framework Core 8
//  Retail Inventory System
// ============================================================
//
//  WHAT IS AN ORM?
//  ---------------
//  An Object-Relational Mapper (ORM) is a tool that lets you
//  interact with a relational database using your programming
//  language's objects instead of writing raw SQL.
//
//  Without ORM (ADO.NET style):
//      SqlCommand cmd = new("SELECT * FROM Products", conn);
//      SqlDataReader reader = cmd.ExecuteReader();
//      while (reader.Read()) { ... }   // manual mapping
//
//  With EF Core ORM:
//      List<Product> products = context.Products.ToList();
//
//  BENEFITS OF EF CORE
//  -------------------
//  1. Productivity  — write C# instead of SQL for most operations
//  2. Type Safety   — compile-time errors instead of runtime SQL bugs
//  3. Migrations    — version-control your database schema
//  4. LINQ Support  — strongly-typed queries with IntelliSense
//  5. Cross-DB      — swap SQL Server for SQLite/PostgreSQL easily
//
//  ADO.NET vs EF CORE
//  ------------------
//  | Feature          | ADO.NET          | EF Core              |
//  |------------------|------------------|----------------------|
//  | Query style      | Raw SQL strings  | LINQ / lambda        |
//  | Object mapping   | Manual           | Automatic            |
//  | Schema changes   | Manual scripts   | Migrations           |
//  | Performance      | Slightly faster  | Slightly more overhead|
//  | Learning curve   | Lower            | Higher (but worth it)|
//
//  EF CORE ARCHITECTURE
//  --------------------
//  Entity Classes  →  DbContext  →  Database Provider  →  SQL Server
//       ↑                ↑                  ↑
//   (Product.cs)  (ApplicationDbContext)  (SqlServer NuGet)
//
//  CODE FIRST WORKFLOW
//  -------------------
//  1. Define entity classes (C# models)
//  2. Create DbContext with DbSet<T> properties
//  3. Run: dotnet ef migrations add InitialCreate
//  4. Run: dotnet ef database update
//  5. Database is created automatically!

Console.WriteLine("==============================================");
Console.WriteLine("  Lab 01 — Understanding ORM & EF Core 8");
Console.WriteLine("==============================================\n");

// ----------------------------------------------------------
// Demonstrate the EF Core architecture by inspecting the
// DbContext without actually connecting to a database.
// ----------------------------------------------------------

Console.WriteLine("EF Core Architecture Overview");
Console.WriteLine("------------------------------");
Console.WriteLine("DbContext Type  : " + typeof(ApplicationDbContext).Name);
Console.WriteLine("Entity Types registered in this context:");

// Retrieve entity types registered via DbSet<T> properties
using var context = new ApplicationDbContext();

foreach (var entityType in context.Model.GetEntityTypes())
{
    Console.WriteLine($"  • {entityType.ClrType.Name}  →  Table: [{entityType.GetTableName()}]");

    // Show mapped columns for each entity
    foreach (var property in entityType.GetProperties())
    {
        Console.WriteLine($"      Column: {property.GetColumnName(),-20} Type: {property.GetColumnType()}");
    }
}

Console.WriteLine();

// ----------------------------------------------------------
// Show entity relationships discovered by EF Core
// ----------------------------------------------------------
Console.WriteLine("Relationships:");
Console.WriteLine("--------------");
foreach (var entityType in context.Model.GetEntityTypes())
{
    foreach (var fk in entityType.GetForeignKeys())
    {
        Console.WriteLine(
            $"  {fk.DeclaringEntityType.ClrType.Name} → {fk.PrincipalEntityType.ClrType.Name}" +
            $"  (FK: {string.Join(", ", fk.Properties.Select(p => p.Name))})"
        );
    }
}

Console.WriteLine();

// ----------------------------------------------------------
// Demonstrate creating entity objects (no DB needed yet)
// ----------------------------------------------------------
Console.WriteLine("Sample Entity Objects (in-memory, no DB connection):");
Console.WriteLine("------------------------------------------------------");

var category = new Category
{
    Id = 1,
    Name = "Electronics",
    Description = "Electronic gadgets and devices"
};

var supplier = new Supplier
{
    Id = 1,
    Name = "TechSupply Co.",
    Email = "contact@techsupply.com",
    Phone = "+1-800-555-0100"
};

var product = new Product
{
    Id = 1,
    Name = "Laptop Pro 15",
    Price = 1299.99m,
    StockQuantity = 50,
    CategoryId = category.Id,
    Category = category,
    SupplierId = supplier.Id,
    Supplier = supplier
};

Console.WriteLine($"  Category : {category.Name} — {category.Description}");
Console.WriteLine($"  Supplier : {supplier.Name} ({supplier.Email})");
Console.WriteLine($"  Product  : {product.Name}  |  Price: ${product.Price:F2}  |  Stock: {product.StockQuantity}");
Console.WriteLine($"             Category → {product.Category?.Name}");
Console.WriteLine($"             Supplier → {product.Supplier?.Name}");

Console.WriteLine();
Console.WriteLine("Code First Workflow Steps:");
Console.WriteLine("--------------------------");
Console.WriteLine("  1. Define entity classes  (Models folder)");
Console.WriteLine("  2. Create ApplicationDbContext  (Data folder)");
Console.WriteLine("  3. dotnet ef migrations add InitialCreate");
Console.WriteLine("  4. dotnet ef database update");
Console.WriteLine("  5. Database & tables are created automatically!");
Console.WriteLine();
Console.WriteLine("Lab 01 complete. Proceed to Lab 02 — Database Context.");
