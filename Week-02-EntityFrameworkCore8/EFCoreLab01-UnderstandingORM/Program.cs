using EFCoreLab01.Data;
using EFCoreLab01.Models;



















































Console.WriteLine("==============================================");
Console.WriteLine("  Lab 01 — Understanding ORM & EF Core 8");
Console.WriteLine("==============================================\n");





Console.WriteLine("EF Core Architecture Overview");
Console.WriteLine("------------------------------");
Console.WriteLine("DbContext Type  : " + typeof(ApplicationDbContext).Name);
Console.WriteLine("Entity Types registered in this context:");

using var context = new ApplicationDbContext();

foreach (var entityType in context.Model.GetEntityTypes())
{
    Console.WriteLine($"  • {entityType.ClrType.Name}  →  Table: [{entityType.GetTableName()}]");

    foreach (var property in entityType.GetProperties())
    {
        Console.WriteLine($"      Column: {property.GetColumnName(),-20} Type: {property.GetColumnType()}");
    }
}

Console.WriteLine();



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
