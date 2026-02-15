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

## Use Cases

### Use Case 1: Create New Student

**Preconditions:**
- API is running and accessible
- User has valid student data (Name, Email, Age)

**Steps:**
1. Send POST request to `/api/students` with student data
2. API validates the input data
3. Repository saves student to in-memory database
4. Dapr publishes "student-created" event to pubsub
5. API returns 201 Created with student details

**Postconditions:**
- New student record exists in database
- Student has unique ID assigned
- Event published for subscribers to consume

---

### Use Case 2: Retrieve All Students

**Preconditions:**
- API is running and accessible
- Database may contain zero or more students

**Steps:**
1. Send GET request to `/api/students`
2. Repository queries all students from database
3. API returns 200 OK with list of students

**Postconditions:**
- No data modifications
- Complete list of students returned

---

### Use Case 3: Retrieve Student by ID

**Preconditions:**
- API is running and accessible
- Student ID is known

**Steps:**
1. Send GET request to `/api/students/{id}`
2. Repository searches for student by ID
3. If found, API returns 200 OK with student details
4. If not found, API returns 404 Not Found

**Postconditions:**
- No data modifications
- Student details returned or error response sent

---

### Use Case 4: Update Existing Student

**Preconditions:**
- API is running and accessible
- Student exists in database
- User has updated student data

**Steps:**
1. Send PUT request to `/api/students/{id}` with updated data
2. API validates the input data
3. Repository updates student in database
4. Dapr publishes "student-updated" event to pubsub
5. API returns 200 OK with updated student details

**Postconditions:**
- Student record updated in database
- Event published for subscribers to consume
- Updated data returned to client

---

### Use Case 5: Delete Student

**Preconditions:**
- API is running and accessible
- Student exists in database

**Steps:**
1. Send DELETE request to `/api/students/{id}`
2. Repository removes student from database
3. Dapr publishes "student-deleted" event to pubsub
4. API returns 204 No Content

**Postconditions:**
- Student record removed from database
- Event published for subscribers to consume
- No content returned to client

---

### Use Case 6: Event-Driven Integration

**Preconditions:**
- API is running with Dapr sidecar
- Subscriber service is configured to listen to student events
- Dapr pubsub component is configured

**Steps:**
1. Student CRUD operation occurs (create/update/delete)
2. API publishes event to Dapr pubsub
3. Dapr routes event to configured topic
4. Subscriber services receive event notification
5. Subscribers process event independently

**Postconditions:**
- Event delivered to all subscribers
- Loosely coupled services remain synchronized
- No direct dependency between services

---

### Use Case 7: Monitor with Aspire Dashboard

**Preconditions:**
- Application running via Aspire AppHost
- Aspire Dashboard accessible at http://localhost:15000

**Steps:**
1. Run `dotnet run --project StudentDaprWithAspire.AppHost`
2. Open browser to http://localhost:15000
3. View application logs, traces, and metrics
4. Monitor API requests and responses
5. Inspect Dapr sidecar communication

**Postconditions:**
- Real-time observability of application behavior
- Performance metrics available for analysis
- Distributed tracing enabled across services

---

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