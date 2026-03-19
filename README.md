# CarsImgApi

A RESTful Web API built with **ASP.NET Core (.NET 9)** for managing vehicle data and inspection images. It provides secure JWT-based authentication against an Oracle database, vehicle order lookups, and Base64-encoded image upload/retrieval with disk persistence.

---

## Table of Contents

- [Features](#features)
- [Tech Stack](#tech-stack)
- [Project Structure](#project-structure)
- [Prerequisites](#prerequisites)
- [Installation](#installation)
- [Configuration](#configuration)
- [Running the Project](#running-the-project)
- [API Endpoints](#api-endpoints)
  - [Authentication](#authentication-v1auth)
  - [Vehicles](#vehicles-v1vehicle)
  - [Vehicle Images](#vehicle-images-v1imgvehicle)
- [Logging](#logging)
- [Testing the API](#testing-the-api)

---

## Features

- **User Authentication** – Login using Oracle database credentials; returns a short-lived JWT token.
- **Vehicle Management** – Retrieve vehicle orders and individual vehicle details per user.
- **Image Management** – Upload up to 7 images per vehicle (Base64 → JPG), and retrieve them back as Base64.
- **Pagination** – Browse vehicle image records with page/limit controls.
- **Centralized Error Handling** – Global middleware logs every unhandled exception with a unique tracking ID.
- **Swagger / OpenAPI** – Interactive API documentation available out of the box.

---

## Tech Stack

| Component | Technology |
|-----------|------------|
| Framework | ASP.NET Core / .NET 9 |
| Database | Oracle (via `Oracle.ManagedDataAccess.Core 3.21.130`) |
| ORM | Dapper 2.1.72 |
| Authentication | JWT Bearer (`Microsoft.AspNetCore.Authentication.JwtBearer 7.0.17`) |
| Logging | Serilog 4.3.1 (Console + daily rolling file) |
| API Docs | Swashbuckle / Swagger 6.4.0 |

---

## Project Structure

```
CarsImgApi/
├── Controllers/
│   ├── AuthController.cs          # Login / logout endpoints
│   ├── VehicleController.cs       # Vehicle order endpoints
│   └── ImgVehicleController.cs    # Vehicle image endpoints
├── Models/
│   ├── Domain/                    # Core business entities
│   └── DTO/                       # Request / Response DTOs
├── Repository/
│   ├── Interface/                 # Data-access contracts
│   └── Implementation/            # Dapper + Oracle implementations
├── services/
│   ├── AuthService.cs             # JWT generation & Oracle auth
│   ├── CreateImageService.cs      # Base64 ↔ JPG file conversion
│   └── DecryptService.cs          # JWT token decryption utility
├── Program.cs                     # Entry point, DI & middleware setup
├── appsettings.json               # Configuration template (no secrets)
└── appsettings.Development.json   # Development configuration
```

---

## Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- Access to an **Oracle Database** instance
- (Optional) Visual Studio 2022, VS Code, or JetBrains Rider

---

## Installation

```bash
# 1. Clone the repository
git clone https://github.com/EcossB/CarsImgApi.git
cd CarsImgApi

# 2. Restore NuGet packages
dotnet restore
```

---

## Configuration

All settings live in `appsettings.json`. **Never commit real credentials.** Update the placeholder values below before running the project, or override them via environment variables (e.g. `ConnectionStrings__OracleDb`):

```json
{
  "ConnectionStrings": {
    "OracleDb": "User Id=<db_user>; Password=<db_password>; Data Source=<host>:<port>/<service>",
    "OracleDbLogin": "User Id={UserID}; Password={Password}; Data Source=<host>:<port>/<service>"
  },
  "DiskRoute": "imagenes",
  "Jwt": {
    "Key": "<your-secret-signing-key-min-32-chars>",
    "Issuer": "http://<api-host>:<api-port>",
    "Audience": "http://<frontend-host>:<frontend-port>"
  },
  "Cors": {
    "AllowedOrigins": [ "http://<frontend-host>:<frontend-port>" ]
  }
}
```

| Key | Description |
|-----|-------------|
| `ConnectionStrings.OracleDb` | Oracle connection string used for vehicle/image queries |
| `ConnectionStrings.OracleDbLogin` | Template string; `{UserID}` and `{Password}` are replaced at login time |
| `DiskRoute` | Relative or absolute path where vehicle JPG images are stored on disk |
| `Jwt:Key` | HMAC-SHA256 secret key used to sign and validate JWT tokens (min 32 chars) |
| `Jwt:Issuer` | Expected token issuer (should match the API base URL) |
| `Jwt:Audience` | Expected token audience (should match the frontend URL) |
| `Cors:AllowedOrigins` | Allowed origins for CORS in Production. In Development all origins are permitted. |

---

## Running the Project

```bash
# Development
dotnet run

# Specific environment
dotnet run --environment Production
```

The API will start on `http://localhost:5249` by default (configurable in `launchSettings.json`).

Interactive Swagger docs are available at:
```
http://localhost:5249/swagger/index.html
```

---

## API Endpoints

### Authentication (`/v1/Auth`)

#### `POST /v1/Auth` — Login
Authenticates against Oracle using the supplied credentials and returns a JWT token (valid for **55 minutes**).

**Request body:**
```json
{
  "userName": "oracle_user",
  "password": "oracle_password"
}
```

**Response `200 OK`:**
```json
{
  "usuarioOracle": "oracle_user",
  "token": "<JWT>"
}
```

---

#### `POST /v1/Auth/logout` — Logout
Ends the current session. Requires a valid Bearer token.

**Request body:**
```json
{
  "userName": "oracle_user"
}
```

**Response `200 OK`:**
```json
{
  "message": "Sección Cerrada Con Exito!"
}
```

---

### Vehicles (`/v1/Vehicle`)

> All endpoints require `Authorization: Bearer <token>` header.

#### `GET /v1/Vehicle/orders/{user}` — Get vehicle orders
Returns all vehicle orders associated with the given Oracle user.

**Response `200 OK`:**
```json
[
  {
    "compania": "string",
    "num_orden": 123,
    "sucursal": "string",
    "nombre": "Customer Name",
    "marca": "Brand",
    "modelo": "Model",
    "placa": "ABC-123",
    "fecha_orden": "2024-01-01T00:00:00"
  }
]
```

---

#### `GET /v1/Vehicle/chasis/{requestChasis}/user{requestUser}` — Get vehicle by plate
Returns a single vehicle record matching the given license plate for the specified user.

**Response `200 OK`:** Single vehicle object (same schema as above).  
**Response `400 Bad Request`:** Vehicle not found.

---

### Vehicle Images (`/v1/ImgVehicle`)

> All endpoints require `Authorization: Bearer <token>` header.

#### `POST /v1/ImgVehicle/addImage` — Upload images
Accepts up to 7 Base64-encoded images, saves them as JPG files on disk, and stores their paths in the database.

**Request body:**
```json
{
  "compania": "string",
  "sucursal": "string",
  "num_orden": 123,
  "img_lateral_derecho": "data:image/jpeg;base64,/9j/...",
  "img_lateral_izquierdo": "data:image/jpeg;base64,/9j/...",
  "img_frontal": "data:image/jpeg;base64,/9j/...",
  "img_trasero": "data:image/jpeg;base64,/9j/...",
  "img_anexo1": "data:image/jpeg;base64,/9j/...",
  "img_anexo2": "data:image/jpeg;base64,/9j/...",
  "img_anexo3": "data:image/jpeg;base64,/9j/...",
  "kilometros": 15000,
  "placa": "ABC-123",
  "usuario": "oracle_user"
}
```

**Response `200 OK`:**
```json
{ "message": "New Vehicle images Save!" }
```

---

#### `GET /v1/ImgVehicle/GetAll/{user}` — Get all vehicle images
Returns all vehicle image records for the given user, with images encoded as Base64 strings.

---

#### `GET /v1/ImgVehicle/getByNumOrder/{num_order}` — Get images by order number
Returns a single vehicle image record by order number.

**Response `400 Bad Request`:** Record not found.

---

#### `GET /v1/ImgVehicle/pagination/user{user}/page{page}/limit{limit}` — Paginated images
Returns a paginated list of vehicle image records.

**Response `200 OK`:**
```json
{
  "imgCars": [ /* array of image records */ ],
  "pages": 5
}
```

---

#### `GET /v1/ImgVehicle/get4first/user{user}` — Get first 4 images
Returns the first 4 vehicle image records for the given user.

---

## Logging

Serilog is configured to write logs to two sinks:

| Sink | Minimum Level | Location |
|------|--------------|----------|
| Console | Information | Standard output |
| File | Error | `logs/errores-YYYYMMDD.txt` (daily rolling) |

Unhandled exceptions are caught by global middleware, logged with a **unique error ID**, and a generic `500` response is returned to the client.

---

## Testing the API

No automated test suite is included. You can explore and test the API using:

1. **Swagger UI** – `http://localhost:5249/swagger/index.html`
2. **REST Client (VS Code)** – Open `WebApplication1.http` and send requests directly from the editor.
3. **cURL / Postman** – Import the OpenAPI spec from `http://localhost:5249/swagger/v1/swagger.json`.

**Example login with cURL:**
```bash
curl -X POST http://localhost:5249/v1/Auth \
  -H "Content-Type: application/json" \
  -d '{"userName":"your_user","password":"your_pass"}'
```

**Example authenticated request:**
```bash
curl -X GET http://localhost:5249/v1/Vehicle/orders/your_user \
  -H "Authorization: Bearer <token>"
```