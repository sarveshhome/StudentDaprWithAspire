# StudentDaprWithAspire

.NET Core 10.0 Web API with Clean Architecture, Repository Pattern, and Dapr Integration

## Project Structure

```
StudentDaprWithAspire/
├── StudentDaprWithAspire.API/          # Presentation Layer
├── StudentDaprWithAspire.Application/  # Application Layer
├── StudentDaprWithAspire.Domain/       # Domain Layer
└── StudentDaprWithAspire.Infrastructure/ # Infrastructure Layer
```

## Architecture

- **Clean Architecture**: Separation of concerns with distinct layers
- **Repository Pattern**: Data access abstraction
- **Dapr Integration**: Event-driven pub/sub messaging

## Features

- CRUD operations for Student entity
- In-Memory database (EF Core)
- Dapr pub/sub events on create, update, delete
- Swagger/OpenAPI documentation

## API Endpoints

- `GET /api/students` - Get all students
- `GET /api/students/{id}` - Get student by ID
- `POST /api/students` - Create new student
- `PUT /api/students/{id}` - Update student
- `DELETE /api/students/{id}` - Delete student

## Run the Application

```bash
dotnet run --project StudentDaprWithAspire.API
```

## Run with Aspire

```bash
dotnet run --project StudentDaprWithAspire.AppHost
```

Aspire Dashboard: http://localhost:15000

## Run with Dapr

```bash
dapr run --app-id student-api --app-port 5000 --dapr-http-port 3500 -- dotnet run --project StudentDaprWithAspire.API
```

## Build

```bash
dotnet build
```