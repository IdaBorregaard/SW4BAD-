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