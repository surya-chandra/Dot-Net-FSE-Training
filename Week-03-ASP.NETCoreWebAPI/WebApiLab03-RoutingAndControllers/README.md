# Lab 03 — Routing & Controllers

## Objective

Implement attribute routing, route parameters, query parameters, and a
complete CRUD API with DTOs for the Retail Inventory System.

---

## Concepts Covered

- Attribute routing with `[Route]` and `[HttpGet/Post/Put/Delete]`
- `[controller]` token in route templates
- Route parameters: `{id:int}`, `{categoryId:int}`
- Route constraints: `:int`, `:alpha`, `:guid`
- Query parameters with `[FromQuery]`
- `[FromBody]` — binding JSON request body
- Data Transfer Objects (DTOs) — request vs response shapes
- Manual DTO mapping (no AutoMapper)
- Nested routes: `/api/products/category/{categoryId}`
- Search endpoint with multiple optional query parameters

---

## Folder Structure

```
WebApiLab03-RoutingAndControllers/
├── Controllers/
│   └── ProductsController.cs   — Full CRUD + search + category filter
├── DTOs/
│   └── ProductDtos.cs          — ProductRequestDto, ProductResponseDto
├── Models/
│   ├── Category.cs
│   ├── Supplier.cs
│   └── Product.cs
├── Data/
│   └── ApplicationDbContext.cs
├── Interfaces/
│   └── IProduct.cs
├── Repositories/
│   └── ProductRepository.cs
├── Services/
│   └── ProductService.cs
├── Program.cs
├── appsettings.json
├── WebApiLab03.csproj
└── README.md
```

---

## How to Run

```bash
cd WebApiLab03-RoutingAndControllers
dotnet restore
dotnet run
```

Navigate to: `https://localhost:{port}/swagger`

---

## API Endpoints

| Method | Route | Description |
|--------|-------|-------------|
| GET | `/api/products` | Get all products |
| GET | `/api/products/{id}` | Get product by ID |
| GET | `/api/products/category/{categoryId}` | Get products by category |
| GET | `/api/products/search?name=&minPrice=&maxPrice=` | Search products |
| POST | `/api/products` | Create a new product |
| PUT | `/api/products/{id}` | Update a product |
| DELETE | `/api/products/{id}` | Delete a product |

### Search Examples

```
GET /api/products/search?name=laptop
GET /api/products/search?minPrice=50&maxPrice=200
GET /api/products/search?name=keyboard&maxPrice=100
```

### POST / PUT Request Body

```json
{
  "name": "Bluetooth Speaker",
  "price": 49.99,
  "stockQuantity": 80,
  "categoryId": 1,
  "supplierId": 1
}
```

---

## Learning Outcomes

- Configure attribute routing with route templates
- Use route parameters with type constraints
- Bind query string parameters with `[FromQuery]`
- Create and use DTOs to shape API input/output
- Implement a search endpoint with multiple optional filters
- Build a complete RESTful CRUD API
