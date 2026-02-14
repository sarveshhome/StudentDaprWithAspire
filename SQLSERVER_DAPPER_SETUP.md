# SQL Server with Dapper Setup

## Prerequisites

SQL Server running on localhost with Windows Authentication or update connection string in `appsettings.json`

## Database Setup

### Option 1: Run SQL Script
```bash
sqlcmd -S localhost -i database-setup.sql
```

### Option 2: Manual Setup
```sql
CREATE DATABASE StudentDb;
GO

USE StudentDb;
GO

CREATE TABLE Students (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL
);
GO
```

## Configuration

Connection string in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=StudentDb;Integrated Security=true;TrustServerCertificate=true;"
  }
}
```

For SQL Authentication:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=StudentDb;User Id=sa;Password=YourPassword;TrustServerCertificate=true;"
  }
}
```

## Run the Application

```bash
dotnet run --project StudentDaprWithAspire.API
```

## Implementation Details

### Dapper Usage
- **QueryAsync** - SELECT queries returning multiple rows
- **QueryFirstOrDefaultAsync** - SELECT single row
- **ExecuteScalarAsync** - INSERT with OUTPUT INSERTED.Id
- **ExecuteAsync** - UPDATE and DELETE operations

### Repository Pattern
All database operations use Dapper for lightweight, high-performance data access:

```csharp
// Example: Get All Students
public async Task<IEnumerable<Student>> GetAllAsync()
{
    using var connection = _connectionFactory.CreateConnection();
    return await connection.QueryAsync<Student>("SELECT Id, Name, Email FROM Students");
}
```

## API Endpoints

- `GET /api/students` - Get all students
- `GET /api/students/{id}` - Get student by ID (int)
- `POST /api/students` - Create new student
- `PUT /api/students/{id}` - Update student
- `DELETE /api/students/{id}` - Delete student

## Example Requests

```bash
# Create a student
curl -X POST http://localhost:5000/api/students \
  -H "Content-Type: application/json" \
  -d '{"name":"John Doe","email":"john@example.com"}'

# Get all students
curl http://localhost:5000/api/students

# Get student by ID
curl http://localhost:5000/api/students/1

# Update student
curl -X PUT http://localhost:5000/api/students/1 \
  -H "Content-Type: application/json" \
  -d '{"id":1,"name":"John Updated","email":"john.updated@example.com"}'

# Delete student
curl -X DELETE http://localhost:5000/api/students/1
```

## Technologies Used

- **Dapper** (v2.1.35) - Micro-ORM for .NET
- **Microsoft.Data.SqlClient** (v5.2.2) - SQL Server data provider
- **SQL Server** - Relational database
- **Clean Architecture** - Separation of concerns
- **Repository Pattern** - Data access abstraction
