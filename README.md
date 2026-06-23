# APIKros

ASP.NET Core Web API for managing companies, employees and a four-level organizational hierarchy.

## Technologies

- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- Docker
- FluentValidation
- Scalar / OpenAPI
- TeaPie tests



## Prerequisites

Before running the project, make sure you have installed:

- Docker Desktop
- WSL 2
- Git
- .NET SDK 10

For Windows development, WSL 2 is recommended.

Check installed versions:

```bash
docker --version
dotnet --version
```

## Installation

Clone the repository:

```bash
git clone https://github.com/adamstano5407/APIKros
```

Navigate to the project directory:

```bash
cd APIKros
```

Create an environment file:

```bash
cp .env-example .env
```

Update an environment file: (DB password must be at least 8 chars)

```bash
nano .env
```

## Build and start the containers:

```bash
make up
```
## Fix permissions

Docker normally runs the application as the host user.

If Rider/VisualStudioCode/IntelliSense cannot access .bin or .obj files on the host, run:

```bash
make fix-host-permissions
```

## Database

Apply migrations:

```bash
make migrate
```

---

## Seed Data (only isDevelopment state)

Generate sample data:

```bash
make seed
```

---

## API Documentation

Scalar UI:

```
http://localhost:8080/scalar/v1
```


## Testing

Run all TeaPie tests:

```bash
make test
```

Run a specific collection:

```bash
docker exec -it api teapie test Tests/Employee
```

Run a single test:

```bash
docker exec -it api teapie test Tests/Employee/001-CreateEmployee.tp
```

---



## DB Model


The database is configured using Entity Framework Core Code First.

Main entities:

- Company
- Division
- Project
- Department
- Employee

The organizational hierarchy is:


Company → Division → Project → Department

Each hierarchy node has:

- name
- code
- optional manager

Employees belong to a company and can be assigned as managers.

## Project structure
```text
APIKros
│
├── Controllers
├── Data
├── DTOs
├── Migrations
├── Models
├── Requests
├── Validators
├── Seeders
└──  Tests
```

---

## Deletion Behavior

Deleting a company:

- removes employee manager assignments
- removes employees
- removes divisions
- removes projects
- removes departments

Hierarchy nodes use cascade delete.

Employee → Company relationship uses **Restrict**, preventing accidental deletion and ensuring explicit cleanup.

---
