# Lab 07 — LINQ Queries

## Objective

Demonstrate the full range of LINQ operators available in EF Core 8 using
real-world Retail Inventory System scenarios.

---

## Concepts Covered

| Operator | SQL Equivalent | Purpose |
|----------|---------------|---------|
| `Where` | `WHERE` | Filter rows by condition |
| `Select` | `SELECT` | Project to specific columns / anonymous type |
| `OrderBy` | `ORDER BY ASC` | Sort ascending |
| `OrderByDescending` | `ORDER BY DESC` | Sort descending |
| `GroupBy` | `GROUP BY` | Group rows by a key |
| `Count` | `COUNT(*)` | Count rows |
| `Sum` | `SUM()` | Sum a numeric column |
| `Average` | `AVG()` | Average of a numeric column |
| `Max` | `MAX()` | Maximum value |
| `Min` | `MIN()` | Minimum value |
| `Any` | `EXISTS` | True if at least one row matches |
| `All` | — | True if every row matches |
| `Include` | `JOIN` | Eager load one navigation property |
| `ThenInclude` | `JOIN` | Eager load nested navigation property |
| `Take` | `TOP` / `LIMIT` | Limit result count |

---

## Folder Structure

```
EFCoreLab07-LINQQueries/
├── Models/
│   ├── Category.cs
│   ├── Supplier.cs
│   └── Product.cs
├── Data/
│   └── ApplicationDbContext.cs
├── Program.cs
├── EFCoreLab07.csproj
└── README.md
```

---

## Prerequisites

- .NET 8 SDK
- SQL Server LocalDB

---

## How to Run

```bash
cd EFCoreLab07-LINQQueries
dotnet restore
dotnet run
```

---

## Expected Output

```
==============================================
  Lab 07 — LINQ Queries
==============================================

1. Where — Products priced above $100
--------------------------------------
  Laptop Pro 15                 $1299.99
  4K Monitor 27"                $449.99
  Power Drill                   $119.99
  Mechanical Keyboard           $89.99

2. Select — Project to Name + Price only
-----------------------------------------
  Laptop Pro 15                 $1299.99
  ...

5. GroupBy — Products grouped by Category
------------------------------------------
  [Clothing]  (3 products)
    • Denim Jacket
    • Running Shoes
    • Sports T-Shirt
  [Electronics]  (5 products)
    ...

6. Count — Total and Conditional
----------------------------------
  Total products      : 14
  In stock (qty > 0)  : 14
  Low stock (qty < 50): 2

7. Sum — Total Inventory Value
--------------------------------
  Total inventory value        : $XXX,XXX.XX
  Electronics inventory value  : $XXX,XXX.XX

9. Max / Min — Price Extremes
------------------------------
  Most expensive : Laptop Pro 15               $1299.99
  Cheapest       : Resistance Bands            $14.99

13. ThenInclude — Eager Load Category → Products → Supplier
------------------------------------------------------------
  [Clothing]
    • Denim Jacket              Supplier: FashionHub Ltd.
    • Running Shoes             Supplier: FashionHub Ltd.
    ...
```

---

## Learning Outcomes

- Write LINQ queries that translate to efficient SQL
- Use projection with `Select` to retrieve only needed columns
- Group and aggregate data with `GroupBy`, `Sum`, `Average`, `Count`
- Check existence with `Any` and universal conditions with `All`
- Eager load related entities one and two levels deep with `Include` / `ThenInclude`
- Combine multiple LINQ operators in a single fluent query chain
