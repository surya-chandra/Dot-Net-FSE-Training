# Lab 04 — Insert Data

## Objective

Insert sample Categories, Suppliers, and Products into the Retail Inventory
database using EF Core's `Add()`, `AddRange()`, and `SaveChanges()` methods.

---

## Concepts Covered

- `Add()` — insert a single entity
- `AddRange()` — insert multiple entities in one call
- `SaveChanges()` / `SaveChangesAsync()` — commit all pending changes to the DB
- EF Core Change Tracking states: Added, Unchanged, Modified, Deleted
- Auto-generated primary keys (IDENTITY columns)
- `EnsureCreatedAsync()` — create DB/tables if they don't exist
- `Include()` — eager loading related data when displaying results

---

## Folder Structure

```
EFCoreLab04-InsertData/
├── Models/
│   ├── Category.cs
│   ├── Supplier.cs
│   └── Product.cs
├── Data/
│   └── ApplicationDbContext.cs
├── Program.cs
├── EFCoreLab04.csproj
└── README.md
```

---

## Prerequisites

- .NET 8 SDK
- SQL Server LocalDB

---

## How to Run

```bash
cd EFCoreLab04-InsertData
dotnet restore
dotnet run
```

> The program calls `EnsureCreatedAsync()` so no manual migration step is needed.
> Running it a second time skips seeding and just displays existing records.

---

## Expected Output

```
==============================================
  Lab 04 — Insert Data
==============================================

Step 1 — Insert single Category using Add()
--------------------------------------------
  ✓ Inserted Category: 'Electronics'  (Id = 1)

Step 2 — Insert multiple Categories using AddRange()
-----------------------------------------------------
  ✓ Inserted Category: 'Clothing'      (Id = 2)
  ✓ Inserted Category: 'Home & Garden' (Id = 3)
  ✓ Inserted Category: 'Sports'        (Id = 4)
  ✓ Inserted Category: 'Books'         (Id = 5)

Step 3 — Insert Suppliers using AddRange()
-------------------------------------------
  ✓ Inserted Supplier: 'TechSupply Co.'   (Id = 1)
  ...

Step 4 — Insert Products using AddRange()
------------------------------------------
  ✓ Inserted Product: 'Laptop Pro 15'  (Id = 1)  Price: $1299.99
  ...

==============================================
  All Records in Database
==============================================

CATEGORIES (5 records):
  Id    Name                 Description
  ...

PRODUCTS (10 records):
  Id    Name                      Price   Stock  Category        Supplier
  ...
```

---

## Learning Outcomes

- Use `Add()` and `AddRange()` to insert entities
- Understand EF Core change tracking
- Call `SaveChangesAsync()` to persist data
- Use `Include()` to load related navigation properties
- Seed a database with realistic sample data
