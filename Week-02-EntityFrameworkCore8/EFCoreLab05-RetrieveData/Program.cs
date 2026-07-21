using EFCoreLab05.Data;
using EFCoreLab05.Models;
using Microsoft.EntityFrameworkCore;

















Console.WriteLine("==============================================");
Console.WriteLine("  Lab 05 — Retrieve Data");
Console.WriteLine("==============================================\n");

using var context = new ApplicationDbContext();
await context.Database.EnsureCreatedAsync();
await SeedDataAsync(context);



PrintHeader("1. Find() — Retrieve by Primary Key");

var foundProduct = await context.Products.FindAsync(1);
if (foundProduct is not null)
    Console.WriteLine($"  Found: [{foundProduct.Id}] {foundProduct.Name}  ${foundProduct.Price:F2}");
else
    Console.WriteLine("  Product with Id=1 not found.");

Console.WriteLine();



PrintHeader("2. First() — First Matching Record");

try
{

    var firstElectronics = await context.Products
        .Include(p => p.Category)
        .Where(p => p.Category!.Name == "Electronics")
        .FirstAsync();

    Console.WriteLine($"  First Electronics product: {firstElectronics.Name}  ${firstElectronics.Price:F2}");
}
catch (InvalidOperationException)
{
    Console.WriteLine("  No Electronics products found.");
}

Console.WriteLine();



PrintHeader("3. FirstOrDefault() — First Match or Null");

var cheapProduct = await context.Products
    .FirstOrDefaultAsync(p => p.Price < 25.00m);

Console.WriteLine(cheapProduct is not null
    ? $"  Cheapest product under $25: {cheapProduct.Name}  ${cheapProduct.Price:F2}"
    : "  No products under $25 found.");

var notFound = await context.Products.FirstOrDefaultAsync(p => p.Price > 10000m);
Console.WriteLine(notFound is not null
    ? $"  Found: {notFound.Name}"
    : "  No product over $10,000 found. (FirstOrDefault returned null — safe!)");

Console.WriteLine();



PrintHeader("4. Single() / SingleOrDefault()");

try
{

    var laptop = await context.Products.SingleAsync(p => p.Name == "Laptop Pro 15");
    Console.WriteLine($"  Single result: {laptop.Name}  ${laptop.Price:F2}  Stock: {laptop.StockQuantity}");
}
catch (InvalidOperationException ex)
{
    Console.WriteLine($"  Single() error: {ex.Message}");
}

var yogaMat = await context.Products.SingleOrDefaultAsync(p => p.Name == "Yoga Mat");
Console.WriteLine(yogaMat is not null
    ? $"  SingleOrDefault: {yogaMat.Name}  ${yogaMat.Price:F2}"
    : "  Product not found.");

Console.WriteLine();



PrintHeader("5. ToList() — Retrieve All Records");

var allCategories = await context.Categories.ToListAsync();
Console.WriteLine($"  All Categories ({allCategories.Count} total):");
foreach (var cat in allCategories)
    Console.WriteLine($"    [{cat.Id}] {cat.Name}");

Console.WriteLine();



PrintHeader("6. Where() — Filter Records");

var lowStock = await context.Products
    .Where(p => p.StockQuantity < 60)
    .OrderBy(p => p.StockQuantity)
    .ToListAsync();

Console.WriteLine($"  Products with stock < 60 ({lowStock.Count} found):");
foreach (var p in lowStock)
    Console.WriteLine($"    {p.Name,-25}  Stock: {p.StockQuantity}");

Console.WriteLine();

var midRange = await context.Products
    .Where(p => p.Price >= 50m && p.Price <= 150m)
    .ToListAsync();

Console.WriteLine($"  Products priced $50–$150 ({midRange.Count} found):");
foreach (var p in midRange)
    Console.WriteLine($"    {p.Name,-25}  ${p.Price:F2}");

Console.WriteLine();



PrintHeader("7. OrderBy() / OrderByDescending()");

var byPriceAsc = await context.Products
    .OrderBy(p => p.Price)
    .ToListAsync();

Console.WriteLine("  Products ordered by Price (ascending):");
foreach (var p in byPriceAsc)
    Console.WriteLine($"    {p.Name,-25}  ${p.Price:F2}");

Console.WriteLine();

var byPriceDesc = await context.Products
    .OrderByDescending(p => p.Price)
    .Take(3)   
    .ToListAsync();

Console.WriteLine("  Top 3 most expensive products:");
int rank = 1;
foreach (var p in byPriceDesc)
    Console.WriteLine($"    #{rank++}  {p.Name,-25}  ${p.Price:F2}");

Console.WriteLine();



PrintHeader("8. Combined Query — Where + OrderBy + Include");

var electronicsInStock = await context.Products
    .Include(p => p.Category)
    .Include(p => p.Supplier)
    .Where(p => p.Category!.Name == "Electronics" && p.StockQuantity > 0)
    .OrderBy(p => p.Name)
    .ToListAsync();

Console.WriteLine($"  Electronics in stock ({electronicsInStock.Count} products):");
Console.WriteLine($"  {"Name",-25} {"Price",10} {"Stock",7} {"Supplier",-20}");
Console.WriteLine($"  {new string('-', 65)}");
foreach (var p in electronicsInStock)
    Console.WriteLine($"  {p.Name,-25} ${p.Price,9:F2} {p.StockQuantity,7} {p.Supplier?.Name,-20}");

Console.WriteLine();
Console.WriteLine("Lab 05 complete. Proceed to Lab 06 — Update & Delete.");



static async Task SeedDataAsync(ApplicationDbContext ctx)
{
    if (await ctx.Categories.AnyAsync()) return;

    var electronics = new Category { Name = "Electronics",    Description = "Electronic gadgets and devices" };
    var clothing    = new Category { Name = "Clothing",       Description = "Apparel and accessories" };
    var sports      = new Category { Name = "Sports",         Description = "Sports equipment" };
    ctx.Categories.AddRange(electronics, clothing, sports);
    await ctx.SaveChangesAsync();

    var techSupplier   = new Supplier { Name = "TechSupply Co.",  Email = "orders@techsupply.com",  Phone = "+1-800-555-0100" };
    var fashionSupplier = new Supplier { Name = "FashionHub Ltd.", Email = "sales@fashionhub.com",   Phone = "+1-800-555-0200" };
    var sportSupplier  = new Supplier { Name = "SportZone Corp.", Email = "info@sportzone.com",      Phone = "+1-800-555-0400" };
    ctx.Suppliers.AddRange(techSupplier, fashionSupplier, sportSupplier);
    await ctx.SaveChangesAsync();

    ctx.Products.AddRange(
        new Product { Name = "Laptop Pro 15",       Price = 1299.99m, StockQuantity = 50,  CategoryId = electronics.Id, SupplierId = techSupplier.Id   },
        new Product { Name = "Wireless Mouse",      Price =   29.99m, StockQuantity = 200, CategoryId = electronics.Id, SupplierId = techSupplier.Id   },
        new Product { Name = "Mechanical Keyboard", Price =   89.99m, StockQuantity = 150, CategoryId = electronics.Id, SupplierId = techSupplier.Id   },
        new Product { Name = "4K Monitor 27\"",     Price =  449.99m, StockQuantity = 30,  CategoryId = electronics.Id, SupplierId = techSupplier.Id   },
        new Product { Name = "Running Shoes",       Price =   79.99m, StockQuantity = 100, CategoryId = clothing.Id,    SupplierId = fashionSupplier.Id },
        new Product { Name = "Denim Jacket",        Price =   59.99m, StockQuantity = 75,  CategoryId = clothing.Id,    SupplierId = fashionSupplier.Id },
        new Product { Name = "Yoga Mat",            Price =   24.99m, StockQuantity = 120, CategoryId = sports.Id,      SupplierId = sportSupplier.Id  },
        new Product { Name = "Dumbbells Set 20kg",  Price =   69.99m, StockQuantity = 55,  CategoryId = sports.Id,      SupplierId = sportSupplier.Id  }
    );
    await ctx.SaveChangesAsync();
}

static void PrintHeader(string title)
{
    Console.WriteLine(title);
    Console.WriteLine(new string('-', title.Length));
}
