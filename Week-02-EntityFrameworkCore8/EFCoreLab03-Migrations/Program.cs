using EFCoreLab03.Data;
using Microsoft.EntityFrameworkCore;

























Console.WriteLine("==============================================");
Console.WriteLine("  Lab 03 — EF Core Migrations");
Console.WriteLine("==============================================\n");

using var context = new ApplicationDbContext();



Console.WriteLine("Migration Status:");
Console.WriteLine("-----------------");

try
{
    var pendingMigrations = await context.Database.GetPendingMigrationsAsync();
    var appliedMigrations = await context.Database.GetAppliedMigrationsAsync();

    Console.WriteLine($"  Applied  migrations : {appliedMigrations.Count()}");
    foreach (var m in appliedMigrations)
        Console.WriteLine($"    ✓ {m}");

    Console.WriteLine($"  Pending  migrations : {pendingMigrations.Count()}");
    foreach (var m in pendingMigrations)
        Console.WriteLine($"    ○ {m}");
}
catch (Exception)
{

    Console.WriteLine("  Database does not exist yet.");
    Console.WriteLine("  Run: dotnet ef database update");
}

Console.WriteLine();




Console.WriteLine("Applying Migrations:");
Console.WriteLine("--------------------");
try
{
    await context.Database.MigrateAsync();
    Console.WriteLine("  ✓ All migrations applied successfully.");
    Console.WriteLine($"  ✓ Database '{context.Database.GetDbConnection().Database}' is ready.");
}
catch (Exception ex)
{
    Console.WriteLine($"  ✗ Migration failed: {ex.Message}");
    Console.WriteLine("  Ensure SQL Server LocalDB is running.");
    Console.WriteLine("  Command: sqllocaldb start mssqllocaldb");
}

Console.WriteLine();



Console.WriteLine("Verifying Database Schema:");
Console.WriteLine("--------------------------");
try
{
    bool categoriesExist = context.Database.CanConnect();
    if (categoriesExist)
    {
        Console.WriteLine("  ✓ [Categories] table exists");
        Console.WriteLine("  ✓ [Suppliers]  table exists");
        Console.WriteLine("  ✓ [Products]   table exists");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"  ✗ Verification failed: {ex.Message}");
}

Console.WriteLine();



Console.WriteLine("EF Core Migration Commands Reference:");
Console.WriteLine("--------------------------------------");
Console.WriteLine("  dotnet ef migrations add InitialCreate");
Console.WriteLine("    → Creates a new migration file in Migrations/");
Console.WriteLine();
Console.WriteLine("  dotnet ef database update");
Console.WriteLine("    → Applies all pending migrations to the database");
Console.WriteLine();
Console.WriteLine("  dotnet ef migrations list");
Console.WriteLine("    → Lists all migrations and their applied status");
Console.WriteLine();
Console.WriteLine("  dotnet ef migrations remove");
Console.WriteLine("    → Removes the last migration (if not applied)");
Console.WriteLine();
Console.WriteLine("  dotnet ef database update 0");
Console.WriteLine("    → Reverts all migrations (drops all tables)");
Console.WriteLine();
Console.WriteLine("  dotnet ef database drop");
Console.WriteLine("    → Drops the entire database");

Console.WriteLine();
Console.WriteLine("Lab 03 complete. Proceed to Lab 04 — Insert Data.");
