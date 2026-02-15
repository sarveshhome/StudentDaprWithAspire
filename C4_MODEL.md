# C4 Model - StudentDaprWithAspire

The C4 model provides a hierarchical way to visualize software architecture at different levels of abstraction.

## Level 1: System Context Diagram

Shows how the Student Management System fits into the overall environment.

```
┌─────────────────┐
│                 │
│  End User/API   │
│     Client      │
│                 │
└────────┬────────┘
         │ HTTP/REST
         │
         ▼
┌─────────────────────────────────────────┐
│                                         │
│   Student Management System             │
│   (StudentDaprWithAspire)               │
│                                         │
│   - Manages student records             │
│   - Publishes student events            │
│   - Provides REST API                   │
│                                         │
└────────┬────────────────────────────────┘
         │ Pub/Sub Events
         │
         ▼
┌─────────────────┐
│                 │
│  External       │
│  Subscriber     │
│  Services       │
│                 │
└─────────────────┘
```

**Key Elements:**
- **End User/API Client**: Consumes REST API for student operations
- **Student Management System**: Core system managing student data
- **External Subscriber Services**: Systems that react to student events (notifications, analytics, etc.)

---

## Level 2: Container Diagram

Shows the high-level technical building blocks and how they interact.

```
┌──────────────────────────────────────────────────────────────────┐
│                    Student Management System                      │
│                                                                    │
│  ┌─────────────────┐         ┌──────────────────┐               │
│  │                 │  HTTP   │                  │               │
│  │  Student API    │◄────────┤  Aspire AppHost  │               │
│  │  (.NET Core)    │         │  (Orchestrator)  │               │
│  │                 │         │                  │               │
│  └────────┬────────┘         └──────────────────┘               │
│           │                                                       │
│           │ Uses                                                  │
│           ▼                                                       │
│  ┌─────────────────┐                                             │
│  │                 │                                             │
│  │  Dapr Sidecar   │──────────► Pub/Sub Component               │
│  │                 │            (Event Publishing)               │
│  └─────────────────┘                                             │
│           │                                                       │
│           │ Stores                                                │
│           ▼                                                       │
│  ┌─────────────────┐                                             │
│  │                 │                                             │
│  │  In-Memory DB   │                                             │
│  │  (EF Core)      │                                             │
│  │                 │                                             │
│  └─────────────────┘                                             │
│                                                                    │
└──────────────────────────────────────────────────────────────────┘
```

**Containers:**
1. **Student API** (.NET Core Web API)
   - Technology: ASP.NET Core 10.0
   - Purpose: REST API for student CRUD operations
   - Port: 5000

2. **Dapr Sidecar**
   - Technology: Dapr runtime
   - Purpose: Event publishing, service invocation
   - Port: 3500

3. **In-Memory Database**
   - Technology: EF Core InMemory
   - Purpose: Student data persistence

4. **Aspire AppHost**
   - Technology: .NET Aspire
   - Purpose: Orchestration and observability
   - Dashboard: http://localhost:15000

---

## Level 3: Component Diagram

Shows the internal structure of the Student API container following Clean Architecture.

```
┌────────────────────────────────────────────────────────────────────┐
│                         Student API Container                       │
│                                                                      │
│  ┌──────────────────────────────────────────────────────────────┐  │
│  │                    Presentation Layer                         │  │
│  │  ┌────────────────────────────────────────────────────────┐  │  │
│  │  │  StudentsController                                     │  │  │
│  │  │  - GetAll()                                             │  │  │
│  │  │  - GetById(id)                                          │  │  │
│  │  │  - Create(student)                                      │  │  │
│  │  │  - Update(id, student)                                  │  │  │
│  │  │  - Patch(id, updates)                                   │  │  │
│  │  │  - Delete(id)                                           │  │  │
│  │  └────────────────┬───────────────────────────────────────┘  │  │
│  └───────────────────┼───────────────────────────────────────────┘  │
│                      │ Uses                                          │
│  ┌───────────────────▼───────────────────────────────────────────┐  │
│  │                    Application Layer                          │  │
│  │  ┌────────────────────────────────────────────────────────┐  │  │
│  │  │  IStudentService (Interface)                           │  │  │
│  │  └────────────────────────────────────────────────────────┘  │  │
│  │  ┌────────────────────────────────────────────────────────┐  │  │
│  │  │  StudentService (Implementation)                       │  │  │
│  │  │  - GetAllStudentsAsync()                               │  │  │
│  │  │  - GetStudentByIdAsync(id)                             │  │  │
│  │  │  - CreateStudentAsync(student)                         │  │  │
│  │  │  - UpdateStudentAsync(student)                         │  │  │
│  │  │  - DeleteStudentAsync(id)                              │  │  │
│  │  └────────────────┬───────────────────────────────────────┘  │  │
│  └───────────────────┼───────────────────────────────────────────┘  │
│                      │ Uses                                          │
│  ┌───────────────────▼───────────────────────────────────────────┐  │
│  │                      Domain Layer                             │  │
│  │  ┌────────────────────────────────────────────────────────┐  │  │
│  │  │  Student (Entity)                                      │  │  │
│  │  │  - Id: int                                             │  │  │
│  │  │  - Name: string                                        │  │  │
│  │  │  - Email: string                                       │  │  │
│  │  │  - Age: int                                            │  │  │
│  │  └────────────────────────────────────────────────────────┘  │  │
│  │  ┌────────────────────────────────────────────────────────┐  │  │
│  │  │  IStudentRepository (Interface)                        │  │  │
│  │  └────────────────────────────────────────────────────────┘  │  │
│  └───────────────────┬───────────────────────────────────────────┘  │
│                      │ Implements                                    │
│  ┌───────────────────▼───────────────────────────────────────────┐  │
│  │                  Infrastructure Layer                         │  │
│  │  ┌────────────────────────────────────────────────────────┐  │  │
│  │  │  StudentRepository (Implementation)                    │  │  │
│  │  │  - GetAllAsync()                                       │  │  │
│  │  │  - GetByIdAsync(id)                                    │  │  │
│  │  │  - AddAsync(student)                                   │  │  │
│  │  │  - UpdateAsync(student)                                │  │  │
│  │  │  - DeleteAsync(id)                                     │  │  │
│  │  └────────────────┬───────────────────────────────────────┘  │  │
│  │  ┌────────────────▼───────────────────────────────────────┐  │  │
│  │  │  ApplicationDbContext (EF Core)                        │  │  │
│  │  │  - Students: DbSet<Student>                            │  │  │
│  │  └────────────────────────────────────────────────────────┘  │  │
│  └───────────────────────────────────────────────────────────────┘  │
│                                                                      │
│  ┌───────────────────────────────────────────────────────────────┐  │
│  │                    External Dependencies                      │  │
│  │  ┌────────────────────────────────────────────────────────┐  │  │
│  │  │  DaprClient                                            │  │  │
│  │  │  - PublishEventAsync(pubsub, topic, data)             │  │  │
│  │  └────────────────────────────────────────────────────────┘  │  │
│  └───────────────────────────────────────────────────────────────┘  │
│                                                                      │
└──────────────────────────────────────────────────────────────────────┘
```

**Components:**

### Presentation Layer (StudentDaprWithAspire.API)
- **StudentsController**: REST API endpoints, handles HTTP requests/responses

### Application Layer (StudentDaprWithAspire.Application)
- **IStudentService**: Service interface defining business operations
- **StudentService**: Business logic implementation, orchestrates repository and Dapr

### Domain Layer (StudentDaprWithAspire.Domain)
- **Student**: Core entity with business rules
- **IStudentRepository**: Repository interface for data access abstraction

### Infrastructure Layer (StudentDaprWithAspire.Infrastructure)
- **StudentRepository**: Data access implementation using EF Core
- **ApplicationDbContext**: EF Core database context

### External Dependencies
- **DaprClient**: Publishes events to Dapr pub/sub

---

## Level 4: Code Diagram

Shows the detailed implementation of key components.

### StudentsController - Create Operation Flow

```
┌─────────────────────────────────────────────────────────────────┐
│  StudentsController.Create(Student student)                     │
│                                                                  │
│  1. Receive HTTP POST request with student data                 │
│     ↓                                                            │
│  2. Call _studentService.CreateStudentAsync(student)            │
│     ↓                                                            │
│  ┌──────────────────────────────────────────────────────────┐  │
│  │  StudentService.CreateStudentAsync(student)              │  │
│  │                                                           │  │
│  │  1. Call _repository.AddAsync(student)                   │  │
│  │     ↓                                                     │  │
│  │  ┌────────────────────────────────────────────────────┐  │  │
│  │  │  StudentRepository.AddAsync(student)               │  │  │
│  │  │                                                     │  │  │
│  │  │  1. _context.Students.Add(student)                 │  │  │
│  │  │  2. await _context.SaveChangesAsync()              │  │  │
│  │  │  3. return student (with generated Id)             │  │  │
│  │  └────────────────────────────────────────────────────┘  │  │
│  │     ↓                                                     │  │
│  │  2. return created student                               │  │
│  └──────────────────────────────────────────────────────────┘  │
│     ↓                                                            │
│  3. await _daprClient.PublishEventAsync(                        │
│        "pubsub",                                                 │
│        "student-created",                                        │
│        created)                                                  │
│     ↓                                                            │
│  4. return CreatedAtAction(nameof(GetById),                     │
│        new { id = created.Id },                                 │
│        created)                                                  │
│                                                                  │
└─────────────────────────────────────────────────────────────────┘
```

### Dependency Flow

```
Program.cs
    │
    ├─► Register Services
    │   ├─► AddApplication() → IStudentService, StudentService
    │   ├─► AddInfrastructure() → IStudentRepository, StudentRepository, DbContext
    │   └─► AddDaprClient() → DaprClient
    │
    └─► Configure Middleware
        ├─► UseRouting()
        ├─► UseAuthorization()
        └─► MapControllers()
```

---

## Key Architectural Patterns

### 1. Clean Architecture (Onion Architecture)
- **Domain** (center): Entities and interfaces
- **Application**: Business logic and use cases
- **Infrastructure**: Data access and external services
- **Presentation**: API controllers

**Benefits:**
- Dependency inversion (inner layers don't depend on outer)
- Testability
- Maintainability

### 2. Repository Pattern
- Abstracts data access through IStudentRepository
- StudentRepository implements data operations
- Allows easy switching of data sources

### 3. Dependency Injection
- All dependencies injected via constructor
- Configured in Program.cs
- Promotes loose coupling

### 4. Event-Driven Architecture
- Dapr pub/sub for asynchronous communication
- Events: student-created, student-updated, student-deleted
- Enables microservices integration

---

## Data Flow Example: Create Student

```
1. Client → POST /api/students
   Body: { "name": "John", "email": "john@example.com", "age": 20 }

2. StudentsController.Create()
   ↓
3. StudentService.CreateStudentAsync()
   ↓
4. StudentRepository.AddAsync()
   ↓
5. ApplicationDbContext.SaveChangesAsync()
   ↓ (Student saved with Id=1)
6. Return to StudentService
   ↓
7. Return to Controller
   ↓
8. DaprClient.PublishEventAsync("pubsub", "student-created", student)
   ↓
9. HTTP 201 Created
   Location: /api/students/1
   Body: { "id": 1, "name": "John", "email": "john@example.com", "age": 20 }

10. Dapr → Publishes event to subscribers
```

---

## Technology Stack Summary

| Layer | Technology |
|-------|-----------|
| Presentation | ASP.NET Core 10.0 Web API |
| Application | C# Services |
| Domain | C# Entities & Interfaces |
| Infrastructure | EF Core InMemory, Dapr Client |
| Orchestration | .NET Aspire |
| Event Bus | Dapr Pub/Sub |
| Testing | xUnit, Moq |

---

## Deployment View

```
┌─────────────────────────────────────────────────────────┐
│                    Aspire AppHost                        │
│                                                          │
│  ┌────────────────────────────────────────────────────┐ │
│  │  Student API (Port 5000)                           │ │
│  │  + Dapr Sidecar (Port 3500)                        │ │
│  └────────────────────────────────────────────────────┘ │
│                                                          │
│  ┌────────────────────────────────────────────────────┐ │
│  │  Aspire Dashboard (Port 15000)                     │ │
│  │  - Logs                                            │ │
│  │  - Traces                                          │ │
│  │  - Metrics                                         │ │
│  └────────────────────────────────────────────────────┘ │
│                                                          │
└─────────────────────────────────────────────────────────┘
```

This C4 model provides a complete architectural view of the StudentDaprWithAspire project from system context down to code-level details.
