# Lab 05 — Exception Handling

## Objective

Implement global exception handling middleware, custom exception types,
and structured JSON error responses for the Retail Inventory API.

---

## Concepts Covered

- Global exception handling middleware (`GlobalExceptionMiddleware`)
- Custom exception types: `NotFoundException`, `ValidationException`, `ConflictException`
- Mapping exceptions to HTTP status codes
- Structured JSON error responses with timestamp and path
- `ILogger<T>` — built-in ASP.NET Core logging
- Clean controllers — no try-catch boilerplate
- Middleware pipeline ordering (exception handler must be first)

---

## Exception → HTTP Status Code Mapping

| Exception Type | HTTP Status Code |
|----------------|-----------------|
| `NotFoundException` | 404 Not Found |
| `ValidationException` | 400 Bad Request |
| `ConflictException` | 409 Conflict |
| `ArgumentException` | 400 Bad Request |
| Any other exception | 500 Internal Server Error |

---

## Folder Structure

```
WebApiLab05-ExceptionHandling/
├── Controllers/
│   └── ProductsController.cs       — Clean actions, no try-catch
├── DTOs/
│   └── ProductDtos.cs
├── Exceptions/
│   └── CustomExceptions.cs         — NotFoundException, ValidationException, ConflictException
├── Middleware/
│   └── GlobalExceptionMiddleware.cs — Catches all exceptions, returns JSON
├── Models/
├── Data/
├── Interfaces/
├── Repositories/
├── Services/
│   └── ProductService.cs           — Throws typed exceptions, uses ILogger
├── Program.cs
├── appsettings.json
├── WebApiLab05.csproj
└── README.md
```

---

## How to Run

```bash
cd WebApiLab05-ExceptionHandling
dotnet restore
dotnet run
```

Navigate to: `https://localhost:{port}/swagger`

---

## Error Response Format

All errors return a consistent JSON structure:

```json
{
  "statusCode": 404,
  "message": "Product with Id=999 was not found.",
  "timestamp": "2024-01-15T10:30:00Z",
  "path": "/api/products/999"
}
```

---

## API Endpoints

| Method | Route | Success | Error |
|--------|-------|---------|-------|
| GET | `/api/products` | 200 OK | 500 |
| GET | `/api/products/{id}` | 200 OK | 404 |
| POST | `/api/products` | 201 Created | 400 |
| PUT | `/api/products/{id}` | 200 OK | 400, 404 |
| DELETE | `/api/products/{id}` | 204 No Content | 404 |

---

## Learning Outcomes

- Build a global exception handling middleware
- Define custom exception types for different error scenarios
- Map exceptions to HTTP status codes in one central place
- Use `ILogger<T>` to log errors with structured logging
- Write clean controllers with no exception handling boilerplate
- Return consistent, structured JSON error responses
