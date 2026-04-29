# Assignment 4

- **Group Name:** [Group 8]
- **Members:** [AU713002], [AU701594]

---

## Technical Specifications
- **Runtime Environment:** .NET 10.0 (Minimal APIs)
- **Relational Database:** Microsoft SQL Server (Deployed via Azure SQL Edge for Docker)
- **NoSQL Database:** MongoDB (Mission logs and API request logging)
- **Data Persistence:** Entity Framework Core (Code-First Approach)
- **Authentication:** Microsoft Identity with JWT Bearer tokens
- **Logging Framework:** Serilog with MongoDB Sink
- **API Documentation:** Scalar (OpenAPI 3.1)
- **Background Service:** .NET Worker Service (MissionLogService)

---

## Service Access and Documentation

* **API Base URL:** http://localhost:5011
* **Scalar UI Documentation:** http://localhost:5011/scalar
* **OpenAPI Specification (JSON):** http://localhost:5011/openapi/v1.json

---

## Installation and Deployment

### Prerequisites
* Docker Desktop must be installed and active.

### Run the Full System
The entire system can be started with a single command from the root `assignment4` folder:

```bash
docker compose up --build
```

This starts all four services:
- SQL Server database
- MongoDB
- Web API (http://localhost:5011)
- MissionLogService (background service)

Database migrations and user seeding run automatically on startup.

---

## Authentication

The API uses JWT-based authentication. To access protected endpoints, you must first log in and include the token in the `Authorization` header.

### Login

`POST /api/auth/login`

```json
{
  "username": "manager1",
  "password": "Manager1Pass!"
}
```

### Roles and Access

| Role      | Access                                           |
| --------- | ------------------------------------------------ |
| Manager   | Full access to all endpoints                     |
| Scientist | Full CRUD on Experiments, GET on everything else |
| Astronaut | GET endpoints only                               |

### Default Seeded Users

| Username   | Password        | Role      |
| ---------- | --------------- | --------- |
| manager1   | Manager1Pass!   | Manager   |
| scientist1 | Scientist1Pass! | Scientist |
| astronaut1 | Astronaut1Pass! | Astronaut |

### Using the Token

Add this header to all protected requests:

Authorization: Bearer <your_token>

---

## Background Service (MissionLogService)
The MissionLogService runs as a separate Worker Service. Every 15 seconds it:
1. Calls `GET /api/missions?status=Active` on the Web API
2. Generates a dummy log entry for each active mission
3. Stores the log in MongoDB

Mission logs can be retrieved via:
`GET /api/missions/{id}/logs`

---

## Project Directory Structure
assignment3/
├── compose.yml                  # Docker container orchestration
├── AarhusSpaceProgram/          # Web API project
│   ├── Endpoints/               # Minimal API route definitions
│   ├── Entities/                # Entity Framework entities
│   ├── DTO/                     # Request and Response DTOs
│   ├── Data/                    # DbContext, migrations, and seeding
│   ├── Properties/              # Launch settings
│   ├── Dockerfile               # Web API container definition
│   └── appsettings.json         # Configuration and connection strings
└── MissionLogService/           # Background Worker Service
    ├── Worker.cs                # Main background service logic
    ├── Dockerfile               # Background service container definition
    └── appsettings.json         # Configuration

---

## Postman Testing
A Postman collection is included for testing the Mission Controller endpoints. Import the collection and select the `AarhusSpaceProgram` environment. Run the Login request first to automatically save the JWT token, then run the remaining requests.