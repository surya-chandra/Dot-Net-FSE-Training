# Lab 06 — Update & Delete

## Objective

Implement full update and delete operations on the Retail Inventory System
using EF Core, including proper null-checking and exception handling.

---

## Concepts Covered

- Updating a single property (EF Core only generates SQL for changed columns)
- Updating multiple properties in one `SaveChanges()` call
- Bulk updates using `Where()` + `foreach` + `SaveChanges()`
- `Remove()` — mark an entity for deletion
- Handling invalid IDs gracefully (null checks before update/delete)
- `InvalidOperationException` for not-found scenarios
- EF Core change tracking states: Modified, Deleted

---

## Folder Structure

```
EFCoreLab06-UpdateDelete/
├── Models/
│   ├── Category.cs
│   ├── Supplier.cs
│   └── Product.cs
├── Data/
│   └── ApplicationDbContext.cs
├── Program.cs
├── EFCoreLab06.csproj
└── README.md
```

---

## Prerequisites

- .NET 8 SDK
- SQL Server LocalDB

---

## How to Run

```bash
cd EFCoreLab06-UpdateDelete
dotnet restore
dotnet run
```

---

## Expected Output

```
==============================================
  Lab 06 — Update & Delete
==============================================

1. Update Single Property — Change Product Price
-------------------------------------------------
  Product  : Laptop Pro 15
  Old Price: $1299.99
  New Price: $1199.99
  ✓ Price updated successfully.

2. Update Multiple Properties — Restock a Product
--------------------------------------------------
  Before — Name: Wireless Mouse  Stock: 200  Price: $29.99
  After  — Name: Wireless Mouse Pro  Stock: 250  Price: $34.99
  ✓ Product updated successfully.

3. Bulk Update — 10% Discount on Sports Products
-------------------------------------------------
  Yoga Mat                   $24.99  →  $22.49
  Dumbbells Set 20kg         $69.99  →  $62.99
  ✓ 2 Sports products discounted by 10%.

4. Update with Invalid ID — Null Handling
------------------------------------------
  ✗ Product with Id=9999 does not exist.
  Update skipped — no exception thrown.

5. Delete — Remove a Product
------------------------------
  Inserted temporary product: 'Temporary Product'  (Id = 6)
  ✓ Product 'Temporary Product' (Id = 6) deleted successfully.

6. Delete with Invalid ID — Exception Handling
-----------------------------------------------
  ✗ Caught expected error: Product with Id=-1 not found.
  ✓ Exception handled gracefully — no data corrupted.
```

---

## Learning Outcomes

- Update entity properties and persist with `SaveChanges()`
- Perform bulk updates using LINQ + `foreach`
- Delete entities using `Remove()` + `SaveChanges()`
- Handle missing records without crashing the application
- Understand EF Core's change tracking for Modified and Deleted states
