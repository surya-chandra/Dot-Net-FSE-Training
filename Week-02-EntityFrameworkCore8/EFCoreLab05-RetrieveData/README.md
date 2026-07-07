# Lab 05 — Retrieve Data

## Objective

Demonstrate all major EF Core data retrieval methods using the Retail Inventory
System, with formatted console output for each query result.

---

## Concepts Covered

| Method | Behaviour |
|--------|-----------|
| `Find(id)` | Lookup by PK; checks cache first |
| `First()` | First match; throws if none |
| `FirstOrDefault()` | First match; returns `null` if none |
| `Single()` | Exactly one match; throws if 0 or 2+ |
| `SingleOrDefault()` | Exactly one; returns `null` if none |
| `ToList()` | Executes query, returns `List<T>` |
| `Where(predicate)` | Filters rows (SQL `WHERE`) |
| `OrderBy()` | Sorts ascending |
| `OrderByDescending()` | Sorts descending |
| `Include()` | Eager loads navigation property |
| `Take(n)` | Limits result count (SQL `TOP`) |

---

## Folder Structure

```
EFCoreLab05-RetrieveData/
├── Models/
│   ├── Category.cs
│   ├── Supplier.cs
│   └── Product.cs
├── Data/
│   └── ApplicationDbContext.cs
├── Program.cs
├── EFCoreLab05.csproj
└── README.md
```

---

## Prerequisites

- .NET 8 SDK
- SQL Server LocalDB

---

## How to Run

```bash
cd EFCoreLab05-RetrieveData
dotnet restore
dotnet run
```

---

## Expected Output

```
==============================================
  Lab 05 — Retrieve Data
==============================================

1. Find() — Retrieve by Primary Key
-------------------------------------
  Found: [1] Laptop Pro 15  $1299.99

2. First() — First Matching Record
------------------------------------
  First Electronics product: Laptop Pro 15  $1299.99

3. FirstOrDefault() — First Match or Null
------------------------------------------
  No products under $25 found.
  No product over $10,000 found. (FirstOrDefault returned null — safe!)

4. Single() / SingleOrDefault()
---------------------------------
  Single result: Laptop Pro 15  $1299.99  Stock: 50

5. ToList() — Retrieve All Records
------------------------------------
  All Categories (3 total):
    [1] Electronics
    [2] Clothing
    [3] Sports

6. Where() — Filter Records
-----------------------------
  Products with stock < 60 (2 found):
    4K Monitor 27"            Stock: 30
    Dumbbells Set 20kg        Stock: 55

7. OrderBy() / OrderByDescending()
------------------------------------
  Products ordered by Price (ascending):
    Yoga Mat                  $24.99
    ...

8. Combined Query — Where + OrderBy + Include
----------------------------------------------
  Electronics in stock (4 products):
  Name                       Price   Stock  Supplier
  ...
```

---

## Learning Outcomes

- Choose the correct retrieval method for each scenario
- Understand when `First()` vs `FirstOrDefault()` is appropriate
- Filter data with `Where()` using lambda predicates
- Sort results with `OrderBy()` and `OrderByDescending()`
- Combine multiple LINQ operators in a single query chain
- Eager load related entities with `Include()`
