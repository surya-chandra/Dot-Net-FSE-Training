# Lab 04 — Model Validation

## Objective

Implement comprehensive request validation using Data Annotations on DTOs,
returning proper HTTP 400 responses with descriptive error messages.

---

## Concepts Covered

| Annotation | Purpose |
|------------|---------|
| `[Required]` | Field must be present and non-null/empty |
| `[StringLength(max, MinimumLength=n)]` | String length constraints |
| `[Range(min, max)]` | Numeric value within bounds |
| `[EmailAddress]` | Valid email format |
| `[Phone]` | Valid phone number format |
| `[MinLength(n)]` / `[MaxLength(n)]` | Collection/string length |

- `[ApiController]` automatic 400 response on validation failure
- `ModelState` — the validation state dictionary
- `ProblemDetails` — RFC 7807 standard error response format
- Validation on route parameters (manual checks)
- Business rule validation in the Service layer

---

## Folder Structure

```
WebApiLab04-ModelValidation/
├── Controllers/
│   ├── ProductsController.cs    — [Required], [Range] validation
│   ├── SuppliersController.cs   — [EmailAddress], [Phone] validation
│   └── CategoriesController.cs  — [StringLength] validation
├── DTOs/
│   └── ProductDtos.cs           — All validated DTOs
├── Models/
├── Data/
├── Interfaces/
├── Repositories/
├── Services/
├── Program.cs
├── appsettings.json
├── WebApiLab04.csproj
└── README.md
```

---

## How to Run

```bash
cd WebApiLab04-ModelValidation
dotnet restore
dotnet run
```

Navigate to: `https://localhost:{port}/swagger`

---

## Validation Error Response Example

Sending an invalid POST body:
```json
{ "name": "", "price": -5, "stockQuantity": -1, "categoryId": 0, "supplierId": 0 }
```

Returns `400 Bad Request`:
```json
{
  "type": "https://tools.ietf.org/html/rfc7807",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "errors": {
    "Name": ["Product name is required."],
    "Price": ["Price must be between $0.01 and $999,999.99."],
    "StockQuantity": ["Stock quantity must be between 0 and 100,000."],
    "CategoryId": ["CategoryId must be a positive integer."],
    "SupplierId": ["SupplierId must be a positive integer."]
  }
}
```

---

## API Endpoints

| Method | Route | Description |
|--------|-------|-------------|
| GET | `/api/products` | Get all products |
| GET | `/api/products/{id}` | Get product by ID |
| POST | `/api/products` | Create product (validated) |
| PUT | `/api/products/{id}` | Update product (validated) |
| DELETE | `/api/products/{id}` | Delete product |
| GET | `/api/suppliers` | Get all suppliers |
| POST | `/api/suppliers` | Create supplier (email validated) |
| GET | `/api/categories` | Get all categories |
| POST | `/api/categories` | Create category (length validated) |

---

## Learning Outcomes

- Apply Data Annotations to DTO properties
- Understand how `[ApiController]` automates validation responses
- Return RFC 7807 `ProblemDetails` error responses
- Validate email, phone, range, and string length constraints
- Combine DTO validation with service-layer business rule validation
