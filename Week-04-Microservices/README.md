# Week 04 — Microservices

## Week Overview

This week introduces **Microservices Architecture** and **JWT Authentication**
using ASP.NET Core 8. The lab builds an enterprise-ready Authentication
Microservice that demonstrates how authentication is implemented in a
real-world microservices system.

---

## Microservices Concepts

### What is a Microservice?

A microservice is a small, independently deployable service that:
- Owns a **single business capability**
- Has its **own database** (no shared schema)
- Communicates with other services via **HTTP APIs or messaging**
- Can be **deployed, scaled, and updated independently**

### Monolith vs Microservices

| Aspect | Monolith | Microservices |
|--------|----------|---------------|
| Deployment | Single unit | Independent per service |
| Scaling | Scale entire app | Scale only bottleneck services |
| Technology | Single stack | Each service can use different stack |
| Failure | One failure can crash all | Failures are isolated |
| Complexity | Simpler to start | More operational complexity |

### This Week's Microservice

The **JWT Authentication Microservice** owns:
- User registration and storage
- Password hashing (PBKDF2)
- JWT token issuance
- Token validation
- Role-based access control (Admin / User)

---

## JWT Concepts

### What is JWT?

JSON Web Token (JWT) is an open standard (RFC 7519) for securely transmitting
claims between parties as a digitally signed JSON object.

### JWT Structure

```
eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9   ← Header (Base64)
.
eyJzdWIiOiIxIiwiZW1haWwiOiJ1c2VyQGV4YW1wbGUuY29tIiwicm9sZSI6IlVzZXIifQ  ← Payload (Base64)
.
SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c  ← Signature (HMAC-SHA256)
```

### How JWT Authentication Works

```
1. Client sends credentials  →  POST /api/auth/login
2. Server validates password →  PasswordHasher.VerifyHashedPassword()
3. Server generates token    →  JwtService.GenerateToken(user)
4. Client stores token       →  localStorage / memory
5. Client sends token        →  Authorization: Bearer <token>
6. Server validates token    →  JwtBearer middleware
7. Server checks role        →  [Authorize(Roles = "Admin")]
8. Server returns response   →  200 OK / 401 / 403
```

---

## Lab Descriptions

| Lab | Title | Key Concepts |
|-----|-------|--------------|
| Lab 01 | JWT Authentication | JWT, Register, Login, Role-based Auth, Swagger Bearer |

---

## Folder Structure

```
Week-04-Microservices/
├── MicroserviceLab01-JWTAuthentication/
│   ├── Controllers/             — AuthController, AdminController, UserController
│   ├── Models/                  — User entity
│   ├── DTOs/                    — Request/Response DTOs
│   ├── Services/                — AuthService, JwtService
│   ├── Interfaces/              — IUserRepository, IAuthService, IJwtService
│   ├── Repositories/            — UserRepository (EF Core)
│   ├── Data/                    — ApplicationDbContext
│   ├── Middleware/              — GlobalExceptionMiddleware
│   ├── Helpers/                 — PasswordHelper (PBKDF2)
│   ├── Configuration/           — JwtSettings (Options pattern)
│   ├── Properties/              — launchSettings.json
│   ├── appsettings.json         — Connection string + JWT config
│   ├── Program.cs               — Full DI + JWT + Swagger setup
│   ├── JWTAuthService.csproj
│   └── README.md
├── Screenshots/
└── README.md
```

---

## Prerequisites

- .NET 8 SDK
- Visual Studio 2022 or VS Code
- SQL Server LocalDB
- EF Core CLI: `dotnet tool install --global dotnet-ef`
- Postman (optional — Swagger UI is built in)

---

## SQL Server Setup

The database is created automatically on first run via `EnsureCreated()`.

Default connection string in `appsettings.json`:
```json
"DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=JWTAuthServiceDb;Trusted_Connection=True;TrustServerCertificate=True;"
```

To use a full SQL Server instance, update the connection string:
```json
"DefaultConnection": "Server=YOUR_SERVER;Database=JWTAuthServiceDb;User Id=sa;Password=YOUR_PASSWORD;TrustServerCertificate=True;"
```

---

## Running Instructions

```bash
# Navigate to the lab folder
cd MicroserviceLab01-JWTAuthentication

# Restore NuGet packages
dotnet restore

# Run the application
dotnet run

# Open Swagger UI
# http://localhost:5000/swagger
```

---

## Quick Test Sequence

```
1. POST /api/auth/register  →  Create Admin user (role: "Admin")
2. POST /api/auth/register  →  Create regular user (role: "User")
3. POST /api/auth/login     →  Login as Admin → copy token
4. Click Authorize in Swagger → paste token
5. GET /api/auth/profile    →  200 OK
6. GET /api/admin/dashboard →  200 OK (Admin token)
7. GET /api/user/dashboard  →  403 Forbidden (Admin cannot access User endpoint)
8. POST /api/auth/login     →  Login as User → copy token
9. GET /api/user/dashboard  →  200 OK (User token)
10. GET /api/admin/dashboard →  403 Forbidden (User cannot access Admin endpoint)
```

---

## Screenshots Section

| Screenshot | Description |
|------------|-------------|
| `Register.png` | Successful user registration response |
| `Login.png` | Login response with JWT token |
| `Swagger.png` | Swagger UI with Authorize button |
| `JWTToken.png` | JWT token decoded at jwt.io |
| `Profile.png` | Authenticated profile endpoint response |
| `AdminDashboard.png` | Admin dashboard (200 OK with Admin token) |
| `UserDashboard.png` | User dashboard (200 OK with User token) |

---

## Learning Outcomes

By the end of Week 4, you will be able to:

- Explain microservice architecture and its benefits
- Implement JWT token generation with HMAC-SHA256 signing
- Configure JWT Bearer authentication in ASP.NET Core 8
- Apply role-based authorization with `[Authorize(Roles)]`
- Hash and verify passwords securely using PBKDF2
- Configure Swagger UI with Bearer token support
- Apply the Repository + Service + Interface pattern
- Use the Options pattern for strongly-typed configuration
- Implement and register global exception handling middleware
- Log authentication events using `ILogger<T>`
