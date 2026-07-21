using EFCoreLab04.Data;
using EFCoreLab04.Models;
using Microsoft.EntityFrameworkCore;




















Console.WriteLine("==============================================");
Console.WriteLine("  Lab 04 — Insert Data");
Console.WriteLine("==============================================\n");

using var context = new ApplicationDbContext();

await context.Database.EnsureCreatedAsync();



if (await context.Categories.AnyAsync())
{
    Console.WriteLine("Data already exists. Displaying existing records...\n");
    await DisplayAllRecordsAsync(context);
    return;
}



Console.WriteLine("Step 1 — Insert single Category using Add()");
Console.WriteLine("--------------------------------------------");

var electronics = new Category
{
    Name = "Electronics",
    Description = "Electronic gadgets, devices, and accessories"
};

context.Categories.Add(electronics);   
await context.SaveChangesAsync();       

Console.WriteLine($"  ✓ Inserted Category: '{electronics.Name}'  (Id = {electronics.Id})");
Console.WriteLine();



Console.WriteLine("Step 2 — Insert multiple Categories using AddRange()");
Console.WriteLine("-----------------------------------------------------");

var moreCategories = new List<Category>
{
    new() { Name = "Clothing",    Description = "Apparel, footwear, and accessories" },
    new() { Name = "Home & Garden", Description = "Furniture, tools, and garden supplies" },
    new() { Name = "Sports",      Description = "Sports equipment and outdoor gear" },
    new() { Name = "Books",       Description = "Fiction, non-fiction, and educational books" }
};

context.Categories.AddRange(moreCategories);  
await context.SaveChangesAsync();              

foreach (var cat in moreCategories)
    Console.WriteLine($"  ✓ Inserted Category: '{cat.Name}'  (Id = {cat.Id})");

Console.WriteLine();



Console.WriteLine("Step 3 — Insert Suppliers using AddRange()");
Console.WriteLine("-------------------------------------------");

var suppliers = new List<Supplier>
{
    new() { Name = "TechSupply Co.",    Email = "orders@techsupply.com",   Phone = "+1-800-555-0100" },
    new() { Name = "FashionHub Ltd.",   Email = "sales@fashionhub.com",    Phone = "+1-800-555-0200" },
    new() { Name = "HomeGoods Inc.",    Email = "contact@homegoods.com",   Phone = "+1-800-555-0300" },
    new() { Name = "SportZone Corp.",   Email = "info@sportzone.com",      Phone = "+1-800-555-0400" }
};

context.Suppliers.AddRange(suppliers);
await context.SaveChangesAsync();

foreach (var sup in suppliers)
    Console.WriteLine($"  ✓ Inserted Supplier: '{sup.Name}'  (Id = {sup.Id})");

Console.WriteLine();




Console.WriteLine("Step 4 — Insert Products using AddRange()");
Console.WriteLine("------------------------------------------");

int electronicsId  = electronics.Id;
int clothingId     = moreCategories[0].Id;
int homeGardenId   = moreCategories[1].Id;
int sportsId       = moreCategories[2].Id;

int techSupplierId   = suppliers[0].Id;
int fashionSupplierId = suppliers[1].Id;
int homeSupplierId   = suppliers[2].Id;
int sportSupplierId  = suppliers[3].Id;

var products = new List<Product>
{
    new() { Name = "Laptop Pro 15",       Price = 1299.99m, StockQuantity = 50,  CategoryId = electronicsId, SupplierId = techSupplierId   },
    new() { Name = "Wireless Mouse",      Price =   29.99m, StockQuantity = 200, CategoryId = electronicsId, SupplierId = techSupplierId   },
    new() { Name = "Mechanical Keyboard", Price =   89.99m, StockQuantity = 150, CategoryId = electronicsId, SupplierId = techSupplierId   },
    new() { Name = "4K Monitor 27\"",     Price =  449.99m, StockQuantity = 30,  CategoryId = electronicsId, SupplierId = techSupplierId   },
    new() { Name = "Running Shoes",       Price =   79.99m, StockQuantity = 100, CategoryId = clothingId,    SupplierId = fashionSupplierId },
    new() { Name = "Denim Jacket",        Price =   59.99m, StockQuantity = 75,  CategoryId = clothingId,    SupplierId = fashionSupplierId },
    new() { Name = "Garden Hose 50ft",    Price =   34.99m, StockQuantity = 60,  CategoryId = homeGardenId,  SupplierId = homeSupplierId   },
    new() { Name = "Power Drill",         Price =  119.99m, StockQuantity = 40,  CategoryId = homeGardenId,  SupplierId = homeSupplierId   },
    new() { Name = "Yoga Mat",            Price =   24.99m, StockQuantity = 120, CategoryId = sportsId,      SupplierId = sportSupplierId  },
    new() { Name = "Dumbbells Set 20kg",  Price =   69.99m, StockQuantity = 55,  CategoryId = sportsId,      SupplierId = sportSupplierId  }
};

context.Products.AddRange(products);
await context.SaveChangesAsync();

foreach (var prod in products)
    Console.WriteLine($"  ✓ Inserted Product: '{prod.Name}'  (Id = {prod.Id})  Price: ${prod.Price:F2}");

Console.WriteLine();



await DisplayAllRecordsAsync(context);



static async Task DisplayAllRecordsAsync(ApplicationDbContext ctx)
{
    Console.WriteLine("==============================================");
    Console.WriteLine("  All Records in Database");
    Console.WriteLine("==============================================\n");

    var categories = await ctx.Categories.OrderBy(c => c.Id).ToListAsync();
    Console.WriteLine($"CATEGORIES ({categories.Count} records):");
    Console.WriteLine($"  {"Id",-5} {"Name",-20} {"Description",-45}");
    Console.WriteLine($"  {new string('-', 72)}");
    foreach (var c in categories)
        Console.WriteLine($"  {c.Id,-5} {c.Name,-20} {c.Description ?? "—",-45}");

    Console.WriteLine();

    var suppliers = await ctx.Suppliers.OrderBy(s => s.Id).ToListAsync();
    Console.WriteLine($"SUPPLIERS ({suppliers.Count} records):");
    Console.WriteLine($"  {"Id",-5} {"Name",-22} {"Email",-30} {"Phone",-18}");
    Console.WriteLine($"  {new string('-', 77)}");
    foreach (var s in suppliers)
        Console.WriteLine($"  {s.Id,-5} {s.Name,-22} {s.Email ?? "—",-30} {s.Phone ?? "—",-18}");

    Console.WriteLine();

    var products = await ctx.Products
        .Include(p => p.Category)
        .Include(p => p.Supplier)
        .OrderBy(p => p.Id)
        .ToListAsync();

    Console.WriteLine($"PRODUCTS ({products.Count} records):");
    Console.WriteLine($"  {"Id",-5} {"Name",-25} {"Price",10} {"Stock",7} {"Category",-15} {"Supplier",-20}");
    Console.WriteLine($"  {new string('-', 87)}");
    foreach (var p in products)
        Console.WriteLine(
            $"  {p.Id,-5} {p.Name,-25} ${p.Price,9:F2} {p.StockQuantity,7} " +
            $"{p.Category?.Name ?? "—",-15} {p.Supplier?.Name ?? "—",-20}"
        );

    Console.WriteLine();
    Console.WriteLine("Lab 04 complete. Proceed to Lab 05 — Retrieve Data.");
}
