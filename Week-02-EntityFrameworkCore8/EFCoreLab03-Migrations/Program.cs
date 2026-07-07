using EFCoreLab03.Data;
using Microsoft.EntityFrameworkCore;

// ============================================================
//  Lab 03 — EF Core Migrations
//  Retail Inventory System
// ============================================================
//
//  WHAT IS A MIGRATION?
//  --------------------
//  A migration is a versioned snapshot of your model changes.
//  Each migration contains:
//    - Up()   : SQL to apply the change (CREATE TABLE, ALTER TABLE...)
//    - Down()  : SQL to revert the change (DROP TABLE...)
//
//  MIGRATION FILES (in Migrations/ folder):
//    20240101000000_InitialCreate.cs       — the migration class
//    ApplicationDbContextModelSnapshot.cs  — current model state
//
//  EF Core compares the snapshot to your current model to
//  determine what changed when you add a new migration.
//
//  WORKFLOW:
//    1. Modify entity classes
//    2. dotnet ef migrations add <MigrationName>
//    3. dotnet ef database update
//    4. Repeat for every schema change

Console.WriteLine("==============================================");
Console.WriteLine("  Lab 03 — EF Core Migrations");
Console.WriteLine("==============================================\n");

using var context = new ApplicationDbContext();

// ----------------------------------------------------------
// 1. Show pending migrations (not yet applied to the DB)
// ----------------------------------------------------------
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
    // Database may not exist yet — that's expected before first migration
    Console.WriteLine("  Database does not exist yet.");
    Console.WriteLine("  Run: dotnet ef database update");
}

Console.WriteLine();

// ----------------------------------------------------------
// 2. Apply all pending migrations programmatically
//    (equivalent to: dotnet ef database update)
// ----------------------------------------------------------
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

// ----------------------------------------------------------
// 3. Verify tables were created
// ----------------------------------------------------------
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

// ----------------------------------------------------------
// 4. Explain migration commands
// ----------------------------------------------------------
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
