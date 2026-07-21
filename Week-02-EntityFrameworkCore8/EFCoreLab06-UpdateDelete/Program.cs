using EFCoreLab06.Data;
using EFCoreLab06.Models;
using Microsoft.EntityFrameworkCore;























Console.WriteLine("==============================================");
Console.WriteLine("  Lab 06 — Update & Delete");
Console.WriteLine("==============================================\n");

using var context = new ApplicationDbContext();
await context.Database.EnsureCreatedAsync();
await SeedDataAsync(context);



PrintHeader("1. Update Single Property — Change Product Price");

var laptop = await context.Products.FirstOrDefaultAsync(p => p.Name == "Laptop Pro 15");
if (laptop is not null)
{
    decimal oldPrice = laptop.Price;
    laptop.Price = 1199.99m;   
    await context.SaveChangesAsync();  

    Console.WriteLine($"  Product  : {laptop.Name}");
    Console.WriteLine($"  Old Price: ${oldPrice:F2}");
    Console.WriteLine($"  New Price: ${laptop.Price:F2}");
    Console.WriteLine("  ✓ Price updated successfully.");
}

Console.WriteLine();



PrintHeader("2. Update Multiple Properties — Restock a Product");

var mouse = await context.Products.FirstOrDefaultAsync(p => p.Name == "Wireless Mouse");
if (mouse is not null)
{
    Console.WriteLine($"  Before — Name: {mouse.Name}  Stock: {mouse.StockQuantity}  Price: ${mouse.Price:F2}");

    mouse.Name          = "Wireless Mouse Pro";
    mouse.StockQuantity = 250;
    mouse.Price         = 34.99m;

    await context.SaveChangesAsync();

    Console.WriteLine($"  After  — Name: {mouse.Name}  Stock: {mouse.StockQuantity}  Price: ${mouse.Price:F2}");
    Console.WriteLine("  ✓ Product updated successfully.");
}

Console.WriteLine();



PrintHeader("3. Bulk Update — 10% Discount on Sports Products");

var sportsProducts = await context.Products
    .Include(p => p.Category)
    .Where(p => p.Category!.Name == "Sports")
    .ToListAsync();

foreach (var product in sportsProducts)
{
    decimal oldPrice = product.Price;
    product.Price = Math.Round(product.Price * 0.90m, 2);
    Console.WriteLine($"  {product.Name,-25}  ${oldPrice:F2}  →  ${product.Price:F2}");
}

await context.SaveChangesAsync();
Console.WriteLine($"  ✓ {sportsProducts.Count} Sports products discounted by 10%.");

Console.WriteLine();



PrintHeader("4. Update with Invalid ID — Null Handling");

int invalidId = 9999;
var notFound = await context.Products.FindAsync(invalidId);

if (notFound is null)
{
    Console.WriteLine($"  ✗ Product with Id={invalidId} does not exist.");
    Console.WriteLine("  Update skipped — no exception thrown.");
}

Console.WriteLine();



PrintHeader("5. Delete — Remove a Product");

var tempProduct = new Product
{
    Name          = "Temporary Product",
    Price         = 9.99m,
    StockQuantity = 1,
    CategoryId    = (await context.Categories.FirstAsync()).Id,
    SupplierId    = (await context.Suppliers.FirstAsync()).Id
};
context.Products.Add(tempProduct);
await context.SaveChangesAsync();
Console.WriteLine($"  Inserted temporary product: '{tempProduct.Name}'  (Id = {tempProduct.Id})");

var toDelete = await context.Products.FindAsync(tempProduct.Id);
if (toDelete is not null)
{
    context.Products.Remove(toDelete);
    await context.SaveChangesAsync();
    Console.WriteLine($"  ✓ Product '{toDelete.Name}' (Id = {toDelete.Id}) deleted successfully.");
}

Console.WriteLine();



PrintHeader("6. Delete with Invalid ID — Exception Handling");

try
{
    int badId = -1;
    var productToDelete = await context.Products.FindAsync(badId)
        ?? throw new InvalidOperationException($"Product with Id={badId} not found.");

    context.Products.Remove(productToDelete);
    await context.SaveChangesAsync();
}
catch (InvalidOperationException ex)
{
    Console.WriteLine($"  ✗ Caught expected error: {ex.Message}");
    Console.WriteLine("  ✓ Exception handled gracefully — no data corrupted.");
}

Console.WriteLine();



PrintHeader("7. Final Product List After Updates & Deletes");

var finalProducts = await context.Products
    .Include(p => p.Category)
    .OrderBy(p => p.CategoryId)
    .ThenBy(p => p.Name)
    .ToListAsync();

Console.WriteLine($"  {"Id",-5} {"Name",-25} {"Price",10} {"Stock",7} {"Category",-15}");
Console.WriteLine($"  {new string('-', 67)}");
foreach (var p in finalProducts)
    Console.WriteLine($"  {p.Id,-5} {p.Name,-25} ${p.Price,9:F2} {p.StockQuantity,7} {p.Category?.Name,-15}");

Console.WriteLine();
Console.WriteLine("Lab 06 complete. Proceed to Lab 07 — LINQ Queries.");



static async Task SeedDataAsync(ApplicationDbContext ctx)
{
    if (await ctx.Categories.AnyAsync()) return;

    var electronics = new Category { Name = "Electronics", Description = "Electronic gadgets" };
    var clothing    = new Category { Name = "Clothing",    Description = "Apparel" };
    var sports      = new Category { Name = "Sports",      Description = "Sports equipment" };
    ctx.Categories.AddRange(electronics, clothing, sports);
    await ctx.SaveChangesAsync();

    var techSupplier   = new Supplier { Name = "TechSupply Co.",  Email = "orders@techsupply.com" };
    var fashionSupplier = new Supplier { Name = "FashionHub Ltd.", Email = "sales@fashionhub.com" };
    var sportSupplier  = new Supplier { Name = "SportZone Corp.", Email = "info@sportzone.com" };
    ctx.Suppliers.AddRange(techSupplier, fashionSupplier, sportSupplier);
    await ctx.SaveChangesAsync();

    ctx.Products.AddRange(
        new Product { Name = "Laptop Pro 15",      Price = 1299.99m, StockQuantity = 50,  CategoryId = electronics.Id, SupplierId = techSupplier.Id   },
        new Product { Name = "Wireless Mouse",     Price =   29.99m, StockQuantity = 200, CategoryId = electronics.Id, SupplierId = techSupplier.Id   },
        new Product { Name = "Running Shoes",      Price =   79.99m, StockQuantity = 100, CategoryId = clothing.Id,    SupplierId = fashionSupplier.Id },
        new Product { Name = "Yoga Mat",           Price =   24.99m, StockQuantity = 120, CategoryId = sports.Id,      SupplierId = sportSupplier.Id  },
        new Product { Name = "Dumbbells Set 20kg", Price =   69.99m, StockQuantity = 55,  CategoryId = sports.Id,      SupplierId = sportSupplier.Id  }
    );
    await ctx.SaveChangesAsync();
}

static void PrintHeader(string title)
{
    Console.WriteLine(title);
    Console.WriteLine(new string('-', title.Length));
}
