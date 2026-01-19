# Time-Off

This project will be a basic leave management system. It currently has the JWT based authentication layer.
The auth service is designed to be consumed by other services (e.g. a Leave Management API) via **JWT access tokens**, while **refresh tokens** are managed server-side using Redis.

---

## 🧱 Architecture Overview

### High-level design

```
Client
  |
  |-- Login / Register
  |
Auth API
  |-- JWT Access Tokens (stateless)
  |-- Refresh Tokens (stateful, Redis)
  |
Other APIs (e.g. Leave API)
  |-- JWT validation only
```

### Key principles

- **Access tokens** are short-lived JWTs
- **Refresh tokens** are long-lived, one-time-use tokens stored in Redis
- Token refresh uses **rotation** to prevent replay attacks
- Other services never talk to the Auth database directly

---

## 🔐 Authentication Strategy

### Access Tokens (JWT)
- Short-lived
- Stateless
- Signed using symmetric key
- Contain:
  - `sub` (UserId)
  - `email`
  - `role`

Validated automatically by ASP.NET Core JWT middleware.

### Refresh Tokens
- Long-lived
- Stored in Redis with TTL
- Mapped as:
  `refresh:{token} -> userId`
- **Rotated on every refresh**
- Revoked on logout

Redis is the source of truth for session validity.

---

## 🛠 Tech Stack

- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- JWT (Microsoft IdentityModel)
- Redis (StackExchange.Redis)
- Docker (for Redis in development)

---

## 📦 Project Structure

```
Auth.Api
├── Controllers
├── Services
├── Repositories
├── Models
├── DTOs
├── Data
└── Program.cs
```

---

## 🚀 Setup Instructions

### Prerequisites
- .NET 8 SDK
- SQL Server
- Docker (for Redis)

### 1. Start Redis

```bash
docker run -d --name redis-auth -p 6379:6379 redis
```

### 2. Configure App Settings

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=AuthDb;User Id=...;Password=...;"
  },
  "Jwt": {
    "Key": "DEV_ONLY_SECRET_KEY",
    "Issuer": "Auth.Api",
    "Audience": "Leave.Api",
    "ExpiryMinutes": 60
  },
  "RefreshToken": {
      "ExpiryDays": 7
  },
  "Redis": {
    "ConnectionString": "localhost:6379"
  }
}
```

### 3. Run Migrations

```bash
dotnet ef database update
```

### 4. Run the API

```bash
dotnet run
```

Swagger will be available at `/swagger`.

---

## 📘 API Documentation

### Register User
**POST** `/api/auth/v1/register`

### Login
**POST** `/api/auth/v1/login`

Returns access + refresh tokens.

### Refresh Token
**POST** `/api/auth/v1/refresh`

Rotates refresh token and issues new access token.

### Logout
**POST** `/api/auth/v1/logout`

Revokes refresh token.

---
