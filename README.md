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

## Project structure

The hierarchy consists of:

Company → Division → Project → Department

Each hierarchy node has:

- name
- code
- optional manager

Employees belong to a company and can be assigned as managers.

## Run with Docker

```bash
docker compose up --build
```

---

## Database

Apply migrations:

```bash
docker exec -it api dotnet ef database update
```

---

## Seed Data

Generate sample data:

```bash
docker exec -it api dotnet run -- --seed
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
docker exec -it api teapie test Tests
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



## Project Structure

```
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
├── Tests
└── Docker
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
