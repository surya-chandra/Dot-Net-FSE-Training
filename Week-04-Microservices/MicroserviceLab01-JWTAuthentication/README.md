# MicroserviceLab01 — JWT Authentication Microservice

## Objective

Build an enterprise-ready JWT Authentication Microservice using ASP.NET Core 8,
EF Core 8, and SQL Server. Implement user registration, login, JWT token generation,
and role-based authorization (Admin / User) following Clean Architecture principles.

---

## Microservices Introduction

A **microservice** is a small, independently deployable service that owns a single
business capability. Unlike a monolith where all features live in one application,
microservices are:

| Characteristic | Description |
|----------------|-------------|
| Single Responsibility | Each service does one thing well |
| Independent Deployment | Deploy without affecting other services |
| Own Database | Each service owns its data — no shared schema |
| Communicate via APIs | Services talk over HTTP/gRPC/messaging |
| Independently Scalable | Scale only the services under load |

This service owns **authentication only** — user registration, login, and token issuance.

---

## JWT Authentication

**JSON Web Token (JWT)** is an open standard (RFC 7519) for securely transmitting
information between parties as a JSON object.

### JWT Structure

```
Header.Payload.Signature
```

| Part | Content |
|------|---------|
| Header | Algorithm (HS256) and token type (JWT) |
| Payload | Claims: userId, email, role, expiry, issuer |
| Signature | HMAC-SHA256(header + payload, secretKey) |

### Claims in this service

| Claim | Value |
|-------|-------|
| `sub` | User ID |
| `email` | User email |
| `name` | Full name |
| `role` | Admin or User |
| `jti` | Unique token ID |
| `exp` | Expiry timestamp |

---

## Authentication Flow

```
Client                    API                      Database
  │                        │                           │
  │── POST /api/auth/login ─►                          │
  │   { email, password }  │── GetByEmailAsync ───────►│
  │                        │◄── User entity ───────────│
  │                        │                           │
  │                        │── VerifyPassword()        │
  │                        │   (PBKDF2 hash check)     │
  │                        │                           │
  │                        │── GenerateToken()         │
  │                        │   (sign with secret key)  │
  │                        │                           │
  │◄── 200 OK ─────────────│                           │
  │   { token, expiresAt } │                           │
```

---

## Authorization Flow

```
Client                    API Middleware              Controller
  │                        │                           │
  │── GET /api/admin/dashboard                         │
  │   Authorization: Bearer <token>                    │
  │                        │                           │
  │                        │── UseAuthentication()     │
  │                        │   Validate token signature│
  │                        │   Load claims into User   │
  │                        │                           │
  │                        │── UseAuthorization()      │
  │                        │   Check Role = "Admin"    │
  │                        │                           │
  │                        │   ✓ Role matches ─────────►│
  │◄── 200 OK ─────────────│◄── return dashboard ──────│
  │                        │                           │
  │                        │   ✗ Role mismatch         │
  │◄── 403 Forbidden ──────│                           │
```

---

## Project Structure

```
MicroserviceLab01-JWTAuthentication/
├── Controllers/
│   ├── AuthController.cs        — Register, Login, Profile
│   ├── AdminController.cs       — Admin Dashboard, All Users
│   └── UserController.cs        — User Dashboard
├── Models/
│   └── User.cs                  — User entity (Id, FullName, Email, PasswordHash, Role, CreatedAt)
├── DTOs/
│   └── AuthDtos.cs              — RegisterRequestDto, LoginRequestDto, LoginResponseDto,
│                                  UserProfileDto, ApiResponseDto<T>
├── Services/
│   ├── AuthService.cs           — Registration, login, profile business logic
│   └── JwtService.cs            — Token generation, signing, validation
├── Interfaces/
│   └── IAuthInterfaces.cs       — IUserRepository, IAuthService, IJwtService
├── Repositories/
│   └── UserRepository.cs        — EF Core data access for Users table
├── Data/
│   └── ApplicationDbContext.cs  — DbContext with Users DbSet
├── Middleware/
│   └── GlobalExceptionMiddleware.cs — Catches all unhandled exceptions
├── Helpers/
│   └── PasswordHelper.cs        — PBKDF2 password hashing and verification
├── Configuration/
│   └── JwtSettings.cs           — Strongly-typed JWT config binding
├── Properties/
│   └── launchSettings.json      — Dev server URLs
├── appsettings.json             — Connection string + JWT settings
├── Program.cs                   — DI, JWT auth, Swagger, middleware pipeline
├── JWTAuthService.csproj
└── README.md
```

---

## Database Schema

```sql
CREATE TABLE Users (
    Id           INT IDENTITY(1,1) PRIMARY KEY,
    FullName     NVARCHAR(100)  NOT NULL,
    Email        NVARCHAR(200)  NOT NULL UNIQUE,
    PasswordHash NVARCHAR(500)  NOT NULL,
    Role         NVARCHAR(20)   NOT NULL DEFAULT 'User',
    CreatedAt    DATETIME2      NOT NULL DEFAULT GETUTCDATE()
);
```

> The database is created automatically by `EnsureCreated()` on first run.

---

## Packages Used

| Package | Version | Purpose |
|---------|---------|---------|
| `Microsoft.EntityFrameworkCore.SqlServer` | 8.0.0 | SQL Server data access |
| `Microsoft.AspNetCore.Authentication.JwtBearer` | 8.0.0 | JWT token validation middleware |
| `Microsoft.AspNetCore.Identity` | 2.2.0 | PasswordHasher (PBKDF2) |
| `Swashbuckle.AspNetCore` | 6.5.0 | Swagger UI with JWT support |

---

## Prerequisites

- .NET 8 SDK
- SQL Server LocalDB (installed with Visual Studio 2022)
- EF Core CLI: `dotnet tool install --global dotnet-ef`

---

## How to Run

```bash
cd MicroserviceLab01-JWTAuthentication
dotnet restore
dotnet run
```

Navigate to: `http://localhost:5000/swagger`

---

## Testing using Swagger UI

### Step 1 — Register an Admin user
```
POST /api/auth/register
{
  "fullName": "Admin User",
  "email": "admin@example.com",
  "password": "Admin@123",
  "role": "Admin"
}
```
Expected: `201 Created`

### Step 2 — Register a regular User
```
POST /api/auth/register
{
  "fullName": "John Doe",
  "email": "john@example.com",
  "password": "User@123",
  "role": "User"
}
```

### Step 3 — Login and get token
```
POST /api/auth/login
{
  "email": "admin@example.com",
  "password": "Admin@123"
}
```
Copy the `token` value from the response.

### Step 4 — Authorize in Swagger UI
1. Click the **Authorize** button (🔒) at the top of Swagger UI
2. Paste the token in the **Value** field
3. Click **Authorize** → **Close**

### Step 5 — Test protected endpoints
- `GET /api/auth/profile` → 200 OK (your profile)
- `GET /api/admin/dashboard` → 200 OK (Admin token) / 403 (User token)
- `GET /api/user/dashboard` → 200 OK (User token) / 403 (Admin token)

---

## Expected Output

### Register Response (201 Created)
```json
{
  "success": true,
  "message": "User registered successfully.",
  "data": {
    "id": 1,
    "fullName": "Admin User",
    "email": "admin@example.com",
    "role": "Admin",
    "createdAt": "2024-01-15T10:00:00Z"
  },
  "timestamp": "2024-01-15T10:00:00Z"
}
```

### Login Response (200 OK)
```json
{
  "success": true,
  "message": "Login successful.",
  "data": {
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "tokenType": "Bearer",
    "expiresAt": "2024-01-15T11:00:00Z",
    "user": {
      "id": 1,
      "fullName": "Admin User",
      "email": "admin@example.com",
      "role": "Admin"
    }
  }
}
```

### Unauthorized (401)
```json
{
  "success": false,
  "message": "Invalid email or password."
}
```

### Forbidden (403)
```
HTTP 403 Forbidden
(No body — ASP.NET Core returns empty 403 for role mismatch)
```

---

## API Endpoints

| Method | Route | Auth Required | Role |
|--------|-------|--------------|------|
| POST | `/api/auth/register` | No | — |
| POST | `/api/auth/login` | No | — |
| GET | `/api/auth/profile` | Yes | Any |
| GET | `/api/admin/dashboard` | Yes | Admin |
| GET | `/api/admin/users` | Yes | Admin |
| GET | `/api/user/dashboard` | Yes | User |

---

## Learning Outcomes

- Understand microservice architecture and single responsibility
- Implement JWT token generation with claims and signing
- Configure JWT Bearer authentication middleware in ASP.NET Core 8
- Apply role-based authorization with `[Authorize(Roles = "Admin")]`
- Hash passwords securely using PBKDF2 (ASP.NET Core Identity)
- Configure Swagger UI with Bearer token authentication
- Apply the Repository + Service pattern in a microservice
- Use the Options pattern for strongly-typed configuration
- Implement global exception handling middleware
- Log authentication events with `ILogger<T>`
