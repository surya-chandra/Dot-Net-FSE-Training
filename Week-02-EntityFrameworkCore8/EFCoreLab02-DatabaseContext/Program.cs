using EFCoreLab02.Data;















Console.WriteLine("==============================================");
Console.WriteLine("  Lab 02 — Database Context");
Console.WriteLine("==============================================\n");


using var context = new ApplicationDbContext();



Console.WriteLine("DbContext Information:");
Console.WriteLine("----------------------");
Console.WriteLine($"  Context Type     : {context.GetType().Name}");
Console.WriteLine($"  Database Name    : {context.Database.GetDbConnection().Database}");
Console.WriteLine($"  Provider         : {context.Database.ProviderName}");

Console.WriteLine();



Console.WriteLine("Registered DbSets (Tables):");
Console.WriteLine("----------------------------");
foreach (var entityType in context.Model.GetEntityTypes())
{
    var tableName = entityType.GetTableName();
    var columnCount = entityType.GetProperties().Count();
    Console.WriteLine($"  DbSet<{entityType.ClrType.Name,-10}>  →  [{tableName}]  ({columnCount} columns)");
}

Console.WriteLine();



Console.WriteLine("Column Configuration (Fluent API):");
Console.WriteLine("-----------------------------------");
foreach (var entityType in context.Model.GetEntityTypes())
{
    Console.WriteLine($"  [{entityType.GetTableName()}]");
    foreach (var prop in entityType.GetProperties())
    {
        var nullable = prop.IsNullable ? "NULL" : "NOT NULL";
        Console.WriteLine($"    {prop.GetColumnName(),-20} {prop.GetColumnType(),-20} {nullable}");
    }
}

Console.WriteLine();



Console.WriteLine("Foreign Key Relationships:");
Console.WriteLine("--------------------------");
foreach (var entityType in context.Model.GetEntityTypes())
{
    foreach (var fk in entityType.GetForeignKeys())
    {
        var fkColumns = string.Join(", ", fk.Properties.Select(p => p.Name));
        Console.WriteLine(
            $"  [{entityType.GetTableName()}].{fkColumns} " +
            $"→ [{fk.PrincipalEntityType.GetTableName()}]"
        );
    }
}

Console.WriteLine();



Console.WriteLine("Database Connectivity Test:");
Console.WriteLine("---------------------------");
try
{
    bool canConnect = context.Database.CanConnect();
    if (canConnect)
    {
        Console.WriteLine("  ✓ Successfully connected to SQL Server.");
        Console.WriteLine("  ✓ Run 'dotnet ef database update' to create tables.");
    }
    else
    {
        Console.WriteLine("  ✗ Cannot connect. Check your connection string.");
        Console.WriteLine("    Connection: Server=(localdb)\\mssqllocaldb");
        Console.WriteLine("    Ensure SQL Server LocalDB is installed.");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"  ✗ Connection error: {ex.Message}");
    Console.WriteLine("    Tip: Install SQL Server LocalDB via Visual Studio Installer.");
}

Console.WriteLine();
Console.WriteLine("Lab 02 complete. Proceed to Lab 03 — EF Core Migrations.");
