# MongoDB Setup Instructions

## Prerequisites

Make sure MongoDB is running on `localhost:8089`

### Start MongoDB with Docker

```bash
docker run -d -p 8089:27017 --name mongodb mongo:latest
```

## Configuration

MongoDB connection settings are in `appsettings.json`:

```json
{
  "MongoDB": {
    "ConnectionString": "mongodb://localhost:8089",
    "DatabaseName": "StudentDb"
  }
}
```

## Run the Application

### Option 1: Run with .NET CLI
```bash
dotnet run --project StudentDaprWithAspire.API
```

### Option 2: Run with Dapr
```bash
dapr run --app-id student-api --app-port 5000 --dapr-http-port 3500 --components-path ./components -- dotnet run --project StudentDaprWithAspire.API
```

### Option 3: Run with Aspire
```bash
dotnet run --project StudentDaprWithAspire.AppHost
```

## Changes Made

1. **Replaced EF Core InMemory** with **MongoDB.Driver**
2. **Updated Student entity** - Changed `Id` from `int` to `string` with MongoDB attributes
3. **Created MongoDbContext** - Replaced ApplicationDbContext with MongoDB context
4. **Updated Repository** - StudentRepository uses MongoDB FilterDefinitionBuilder for type-safe queries
5. **Updated all interfaces** - Changed ID type from `int` to `string` throughout the application
6. **Added Dapr component** - Created `components/mongodb-statestore.yaml` for Dapr state store

## API Endpoints

All endpoints now use string IDs instead of int:

- `GET /api/students` - Get all students
- `GET /api/students/{id}` - Get student by ID (string)
- `POST /api/students` - Create new student
- `PUT /api/students/{id}` - Update student (string ID)
- `DELETE /api/students/{id}` - Delete student (string ID)

## Example Request

```bash
# Create a student
curl -X POST http://localhost:5000/api/students \
  -H "Content-Type: application/json" \
  -d '{"name":"John Doe","email":"john@example.com"}'

# Get all students
curl http://localhost:5000/api/students

# Get student by ID (use the ID returned from create)
curl http://localhost:5000/api/students/507f1f77bcf86cd799439011
```
