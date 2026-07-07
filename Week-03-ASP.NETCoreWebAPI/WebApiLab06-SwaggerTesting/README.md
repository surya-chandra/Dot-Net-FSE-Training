# Lab 06 — Swagger & API Testing

## Objective

Configure Swagger/OpenAPI with full documentation, XML comments, and
response type annotations. Test all CRUD endpoints interactively via Swagger UI.

---

## Concepts Covered

- Swagger / OpenAPI specification
- `AddSwaggerGen()` — registers Swagger generation services
- `UseSwagger()` — serves the OpenAPI JSON document
- `UseSwaggerUI()` — serves the interactive browser UI
- `OpenApiInfo` — API title, version, description, contact
- `[ProducesResponseType]` — documents possible HTTP responses
- `[Produces]` / `[Consumes]` — documents content types
- XML documentation comments (`/// <summary>`) in Swagger UI
- `<GenerateDocumentationFile>true</GenerateDocumentationFile>` in `.csproj`
- `IncludeXmlComments()` — loads XML docs into Swagger
- `/// <example>` on DTO properties — pre-fills Swagger UI fields

---

## Folder Structure

```
WebApiLab06-SwaggerTesting/
├── Controllers/
│   └── ProductsController.cs   — Full XML docs + [ProducesResponseType]
├── DTOs/
│   └── ProductDtos.cs          — DTOs with <example> annotations
├── Exceptions/
│   └── CustomExceptions.cs
├── Middleware/
│   └── GlobalExceptionMiddleware.cs
├── Models/
├── Data/
├── Interfaces/
├── Repositories/
├── Services/
├── Program.cs                  — Full Swagger configuration
├── appsettings.json
├── WebApiLab06.csproj          — GenerateDocumentationFile=true
└── README.md
```

---

## How to Run

```bash
cd WebApiLab06-SwaggerTesting
dotnet restore
dotnet run
```

Open your browser and navigate to:
```
https://localhost:{port}/swagger
```

---

## Swagger UI Testing Guide

### Step-by-step for each endpoint:

1. **GET /api/products** — Click → Try it out → Execute → See all products
2. **GET /api/products/{id}** — Enter `id=1` → Execute → See one product
3. **GET /api/products/search** — Enter `name=laptop` → Execute → See filtered results
4. **POST /api/products** — Click → Try it out → Edit the JSON body → Execute → See 201 Created
5. **PUT /api/products/{id}** — Enter `id=1` → Edit body → Execute → See updated product
6. **DELETE /api/products/{id}** — Enter `id=1` → Execute → See 204 No Content

### Sample POST body (pre-filled from `<example>` annotations):
```json
{
  "name": "Laptop Pro 15",
  "price": 1299.99,
  "stockQuantity": 50,
  "categoryId": 1,
  "supplierId": 1
}
```

---

## API Endpoints

| Method | Route | Status Codes |
|--------|-------|-------------|
| GET | `/api/products` | 200 |
| GET | `/api/products/{id}` | 200, 404 |
| GET | `/api/products/category/{categoryId}` | 200 |
| GET | `/api/products/search` | 200, 400 |
| POST | `/api/products` | 201, 400 |
| PUT | `/api/products/{id}` | 200, 400, 404 |
| DELETE | `/api/products/{id}` | 204, 404 |

---

## OpenAPI Document

The raw OpenAPI JSON is available at:
```
https://localhost:{port}/swagger/v1/swagger.json
```

---

## Learning Outcomes

- Configure Swagger/OpenAPI in an ASP.NET Core 8 project
- Document endpoints with XML comments and `[ProducesResponseType]`
- Enable XML documentation generation in the `.csproj`
- Use Swagger UI to test all CRUD operations interactively
- Understand the OpenAPI specification format
- Add example values to DTO properties for better Swagger UX
