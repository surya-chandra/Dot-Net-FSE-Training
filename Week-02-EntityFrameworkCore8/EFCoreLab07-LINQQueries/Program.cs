using EFCoreLab07.Data;
using EFCoreLab07.Models;
using Microsoft.EntityFrameworkCore;














Console.WriteLine("==============================================");
Console.WriteLine("  Lab 07 — LINQ Queries");
Console.WriteLine("==============================================\n");

using var context = new ApplicationDbContext();
await context.Database.EnsureCreatedAsync();
await SeedDataAsync(context);



PrintHeader("1. Where — Products priced above $100");

var expensive = await context.Products
    .Where(p => p.Price > 100m)
    .ToListAsync();

foreach (var p in expensive)
    Console.WriteLine($"  {p.Name,-28}  ${p.Price:F2}");

Console.WriteLine();



PrintHeader("2. Select — Project to Name + Price only");

var projection = await context.Products
    .Select(p => new { p.Name, p.Price })
    .ToListAsync();

foreach (var item in projection)
    Console.WriteLine($"  {item.Name,-28}  ${item.Price:F2}");

Console.WriteLine();



PrintHeader("3. OrderBy — Products sorted by Name (A→Z)");

var byName = await context.Products
    .OrderBy(p => p.Name)
    .Select(p => p.Name)
    .ToListAsync();

foreach (var name in byName)
    Console.WriteLine($"  {name}");

Console.WriteLine();



PrintHeader("4. OrderByDescending — Top 5 most expensive");

var top5 = await context.Products
    .OrderByDescending(p => p.Price)
    .Take(5)
    .ToListAsync();

int rank = 1;
foreach (var p in top5)
    Console.WriteLine($"  #{rank++}  {p.Name,-28}  ${p.Price:F2}");

Console.WriteLine();



PrintHeader("5. GroupBy — Products grouped by Category");

var grouped = await context.Products
    .Include(p => p.Category)
    .GroupBy(p => p.Category!.Name)
    .Select(g => new
    {
        CategoryName = g.Key,
        ProductCount = g.Count(),
        Products     = g.Select(p => p.Name).ToList()
    })
    .ToListAsync();

foreach (var group in grouped)
{
    Console.WriteLine($"  [{group.CategoryName}]  ({group.ProductCount} products)");
    foreach (var name in group.Products)
        Console.WriteLine($"    • {name}");
}

Console.WriteLine();



PrintHeader("6. Count — Total and Conditional");

int totalProducts    = await context.Products.CountAsync();
int inStockCount     = await context.Products.CountAsync(p => p.StockQuantity > 0);
int lowStockCount    = await context.Products.CountAsync(p => p.StockQuantity < 50);

Console.WriteLine($"  Total products      : {totalProducts}");
Console.WriteLine($"  In stock (qty > 0)  : {inStockCount}");
Console.WriteLine($"  Low stock (qty < 50): {lowStockCount}");

Console.WriteLine();



PrintHeader("7. Sum — Total Inventory Value");

decimal totalValue = await context.Products
    .SumAsync(p => p.Price * p.StockQuantity);

decimal electronicsValue = await context.Products
    .Include(p => p.Category)
    .Where(p => p.Category!.Name == "Electronics")
    .SumAsync(p => p.Price * p.StockQuantity);

Console.WriteLine($"  Total inventory value        : ${totalValue:N2}");
Console.WriteLine($"  Electronics inventory value  : ${electronicsValue:N2}");

Console.WriteLine();



PrintHeader("8. Average — Mean Price per Category");

var avgByCategory = await context.Products
    .Include(p => p.Category)
    .GroupBy(p => p.Category!.Name)
    .Select(g => new
    {
        Category     = g.Key,
        AveragePrice = g.Average(p => p.Price)
    })
    .OrderBy(x => x.Category)
    .ToListAsync();

foreach (var item in avgByCategory)
    Console.WriteLine($"  {item.Category,-20}  Avg Price: ${item.AveragePrice:F2}");

Console.WriteLine();



PrintHeader("9. Max / Min — Price Extremes");

decimal maxPrice = await context.Products.MaxAsync(p => p.Price);
decimal minPrice = await context.Products.MinAsync(p => p.Price);

var mostExpensive = await context.Products.FirstAsync(p => p.Price == maxPrice);
var cheapest      = await context.Products.FirstAsync(p => p.Price == minPrice);

Console.WriteLine($"  Most expensive : {mostExpensive.Name,-28}  ${maxPrice:F2}");
Console.WriteLine($"  Cheapest       : {cheapest.Name,-28}  ${minPrice:F2}");

Console.WriteLine();



PrintHeader("10. Any — Existence Checks");

bool hasElectronics  = await context.Products
    .Include(p => p.Category)
    .AnyAsync(p => p.Category!.Name == "Electronics");

bool hasOutOfStock   = await context.Products.AnyAsync(p => p.StockQuantity == 0);

bool hasPremium      = await context.Products.AnyAsync(p => p.Price > 1000m);

Console.WriteLine($"  Has Electronics products  : {hasElectronics}");
Console.WriteLine($"  Has out-of-stock products : {hasOutOfStock}");
Console.WriteLine($"  Has premium (>$1000) items: {hasPremium}");

Console.WriteLine();



PrintHeader("11. All — Universal Condition Checks");

bool allInStock      = await context.Products.AllAsync(p => p.StockQuantity > 0);
bool allPositivePrice = await context.Products.AllAsync(p => p.Price > 0m);

Console.WriteLine($"  All products in stock     : {allInStock}");
Console.WriteLine($"  All products have price>0 : {allPositivePrice}");

Console.WriteLine();



PrintHeader("12. Include — Eager Load Category");

var productsWithCategory = await context.Products
    .Include(p => p.Category)
    .OrderBy(p => p.Category!.Name)
    .ThenBy(p => p.Name)
    .ToListAsync();

Console.WriteLine($"  {"Product",-28} {"Category",-15} {"Price",10}");
Console.WriteLine($"  {new string('-', 57)}");
foreach (var p in productsWithCategory)
    Console.WriteLine($"  {p.Name,-28} {p.Category?.Name,-15} ${p.Price,9:F2}");

Console.WriteLine();



PrintHeader("13. ThenInclude — Eager Load Category → Products → Supplier");

var categories = await context.Categories
    .Include(c => c.Products)
        .ThenInclude(p => p.Supplier)
    .OrderBy(c => c.Name)
    .ToListAsync();

foreach (var cat in categories)
{
    Console.WriteLine($"  [{cat.Name}]");
    foreach (var p in cat.Products)
        Console.WriteLine($"    • {p.Name,-28}  Supplier: {p.Supplier?.Name}");
}

Console.WriteLine();





PrintHeader("14. Combined Query — Real-World Scenario");

var result = await context.Products
    .Include(p => p.Category)
    .Include(p => p.Supplier)
    .Where(p => p.Category!.Name == "Electronics"
             && p.Supplier!.Name == "TechSupply Co."
             && p.Price > 50m)
    .OrderByDescending(p => p.Price)
    .Select(p => new
    {
        p.Name,
        p.Price,
        p.StockQuantity,
        CategoryName = p.Category!.Name,
        SupplierName = p.Supplier!.Name
    })
    .ToListAsync();

Console.WriteLine($"  Electronics from TechSupply Co. priced > $50:");
Console.WriteLine($"  {"Name",-28} {"Price",10} {"Stock",7}");
Console.WriteLine($"  {new string('-', 48)}");
foreach (var item in result)
    Console.WriteLine($"  {item.Name,-28} ${item.Price,9:F2} {item.StockQuantity,7}");

Console.WriteLine();
Console.WriteLine("Lab 07 complete. Week 02 — Entity Framework Core 8 finished!");
Console.WriteLine("=============================================================");



static async Task SeedDataAsync(ApplicationDbContext ctx)
{
    if (await ctx.Categories.AnyAsync()) return;

    var electronics = new Category { Name = "Electronics",   Description = "Electronic gadgets and devices" };
    var clothing    = new Category { Name = "Clothing",      Description = "Apparel and accessories" };
    var homeGarden  = new Category { Name = "Home & Garden", Description = "Furniture, tools, garden supplies" };
    var sports      = new Category { Name = "Sports",        Description = "Sports equipment and outdoor gear" };
    ctx.Categories.AddRange(electronics, clothing, homeGarden, sports);
    await ctx.SaveChangesAsync();

    var techSupplier    = new Supplier { Name = "TechSupply Co.",  Email = "orders@techsupply.com",  Phone = "+1-800-555-0100" };
    var fashionSupplier = new Supplier { Name = "FashionHub Ltd.", Email = "sales@fashionhub.com",   Phone = "+1-800-555-0200" };
    var homeSupplier    = new Supplier { Name = "HomeGoods Inc.",  Email = "contact@homegoods.com",  Phone = "+1-800-555-0300" };
    var sportSupplier   = new Supplier { Name = "SportZone Corp.", Email = "info@sportzone.com",     Phone = "+1-800-555-0400" };
    ctx.Suppliers.AddRange(techSupplier, fashionSupplier, homeSupplier, sportSupplier);
    await ctx.SaveChangesAsync();

    ctx.Products.AddRange(

        new Product { Name = "Laptop Pro 15",       Price = 1299.99m, StockQuantity = 50,  CategoryId = electronics.Id, SupplierId = techSupplier.Id    },
        new Product { Name = "Wireless Mouse",      Price =   29.99m, StockQuantity = 200, CategoryId = electronics.Id, SupplierId = techSupplier.Id    },
        new Product { Name = "Mechanical Keyboard", Price =   89.99m, StockQuantity = 150, CategoryId = electronics.Id, SupplierId = techSupplier.Id    },
        new Product { Name = "4K Monitor 27\"",     Price =  449.99m, StockQuantity = 30,  CategoryId = electronics.Id, SupplierId = techSupplier.Id    },
        new Product { Name = "USB-C Hub",           Price =   39.99m, StockQuantity = 180, CategoryId = electronics.Id, SupplierId = techSupplier.Id    },

        new Product { Name = "Running Shoes",       Price =   79.99m, StockQuantity = 100, CategoryId = clothing.Id,    SupplierId = fashionSupplier.Id },
        new Product { Name = "Denim Jacket",        Price =   59.99m, StockQuantity = 75,  CategoryId = clothing.Id,    SupplierId = fashionSupplier.Id },
        new Product { Name = "Sports T-Shirt",      Price =   19.99m, StockQuantity = 300, CategoryId = clothing.Id,    SupplierId = fashionSupplier.Id },

        new Product { Name = "Garden Hose 50ft",    Price =   34.99m, StockQuantity = 60,  CategoryId = homeGarden.Id,  SupplierId = homeSupplier.Id    },
        new Product { Name = "Power Drill",         Price =  119.99m, StockQuantity = 40,  CategoryId = homeGarden.Id,  SupplierId = homeSupplier.Id    },
        new Product { Name = "LED Desk Lamp",       Price =   44.99m, StockQuantity = 90,  CategoryId = homeGarden.Id,  SupplierId = homeSupplier.Id    },

        new Product { Name = "Yoga Mat",            Price =   24.99m, StockQuantity = 120, CategoryId = sports.Id,      SupplierId = sportSupplier.Id   },
        new Product { Name = "Dumbbells Set 20kg",  Price =   69.99m, StockQuantity = 55,  CategoryId = sports.Id,      SupplierId = sportSupplier.Id   },
        new Product { Name = "Resistance Bands",    Price =   14.99m, StockQuantity = 250, CategoryId = sports.Id,      SupplierId = sportSupplier.Id   }
    );
    await ctx.SaveChangesAsync();
}

static void PrintHeader(string title)
{
    Console.WriteLine(title);
    Console.WriteLine(new string('-', title.Length));
}
