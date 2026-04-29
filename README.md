# Space Agency Management System - Assignment 3

## Project Description
This application serves as a centralized Management API for a Space Agency. It is designed to coordinate missions, personnel (Astronauts, Scientists, and Managers), and launch logistics. The system architecture utilizes a hybrid database approach, leveraging Microsoft SQL Server for high-integrity relational data and MongoDB for asynchronous event logging.

### Team Information
* **Group Name:** [Group 8]
* **Members:** [AU713002], [AU701594]

---

## Technical Specifications
* **Runtime Environment:** .NET 10.0 (Minimal APIs)
* **Relational Database:** Microsoft SQL Server (Deployed via Azure SQL Edge for Docker)
* **NoSQL Database:** MongoDB (Utilized for Requirement E Logging)
* **Data Persistence:** Entity Framework Core (Code-First Approach)
* **Logging Framework:** Serilog with MongoDB Sink
* **API Documentation:** Scalar (OpenAPI 3.1)

---

## Service Access and Documentation

The application provides a standardized interface for interaction and documentation. Once the service is running, it can be accessed via the following endpoints:

* **API Base URL:** http://localhost:5011
* **Scalar UI Documentation:** http://localhost:5011/scalar
* **OpenAPI Specification (JSON):** http://localhost:5011/openapi/v1.json

---

## Installation and Deployment

### Prerequisites
* Docker Desktop must be installed and active.
* .NET 10 SDK must be installed on the host machine.

### Step 1: Infrastructure Deployment
Initialize the SQL Server and MongoDB containers using the provided Docker Compose configuration:
```docker compose up -d```

### Step 2: Database Initialization
Apply Entity Framework Migrations to synchronize the relational schema and execute data seeding:
```dotnet ef database update```

### Step 3: Application Execution
Launch the API server:
```dotnet run```

---

## Project Directory Structure
├── Endpoints/       # Minimal API route definitions and logic
├── Models/          # Entity Framework Entities (SQL Schema)
├── DTOs/            # Request and Response Data Transfer Objects
├── Data/            # DbContext and Data Seeding Logic
├── Properties/      # Development profiles and launch settings
├── appsettings.json # Environment configurations and connection strings
└── compose.yml      # Docker container orchestration



Set id to 1 in the URL
Add the Authorization header:

Key: Authorization
Value: Bearer <your token>


Set the request body to { "status": 1 }
Click Send
Repeat with 2, 3, and 4


#### Get token:
{
  "username": "manager1",
  "password": "Manager1Pass!"
}


#### Token: 
Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoibWFuYWdlcjEiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjJjNDBmYTZhLTg3ZDctNGI0MC1hOWMyLTMyYmI1MzY2OGVmYSIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6Ik1hbmFnZXIiLCJleHAiOjE3Nzc0OTE1ODQsImlzcyI6IkFhcmh1c1NwYWNlUHJvZ3JhbSIsImF1ZCI6IkFhcmh1c1NwYWNlUHJvZ3JhbSJ9.43_F4ZWdp9CD7IRd9PzQa5CMVxaoW4eCr-Gm9kOCWcI