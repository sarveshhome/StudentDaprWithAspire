# Blazor + API Integration Guide

## Architecture Overview

```
┌─────────────────────────────────────────────────────────────┐
│                        User Browser                          │
│                                                              │
│  ┌────────────────────────────────────────────────────────┐ │
│  │         Blazor Server Application                      │ │
│  │         (StudentDaprWithAspire.WebBlazor)             │ │
│  │                                                        │ │
│  │  Port: 5001 (HTTP) / 7001 (HTTPS)                    │ │
│  └────────────────────────────────────────────────────────┘ │
│                           │                                  │
│                           │ SignalR (WebSocket)              │
│                           ▼                                  │
└─────────────────────────────────────────────────────────────┘
                            │
                            │ HTTP REST API Calls
                            ▼
┌─────────────────────────────────────────────────────────────┐
│              Student API                                     │
│              (StudentDaprWithAspire.API)                    │
│                                                              │
│              Port: 5000                                      │
│                                                              │
│  Endpoints:                                                  │
│  - GET    /api/students                                     │
│  - GET    /api/students/{id}                                │
│  - POST   /api/students                                     │
│  - PUT    /api/students/{id}                                │
│  - DELETE /api/students/{id}                                │
└─────────────────────────────────────────────────────────────┘
                            │
                            ▼
                    ┌───────────────┐
                    │  In-Memory DB │
                    └───────────────┘
```

---

## Communication Flow

### 1. Get All Students

```
Browser → Blazor Server (Students.razor)
         ↓
    StudentApiService.GetAllStudentsAsync()
         ↓
    HttpClient.GetFromJsonAsync("/api/students")
         ↓
    HTTP GET → http://localhost:5000/api/students
         ↓
    Student API → StudentsController.GetAll()
         ↓
    StudentService.GetAllStudentsAsync()
         ↓
    StudentRepository.GetAllAsync()
         ↓
    EF Core → In-Memory Database
         ↓
    JSON Response ← List<Student>
         ↓
    Blazor Server ← Deserialize
         ↓
    Browser ← Render Table
```

### 2. Create Student

```
Browser → User fills form in CreateStudent.razor
         ↓
    Form Submit → HandleSubmit()
         ↓
    StudentApiService.CreateStudentAsync(student)
         ↓
    HttpClient.PostAsJsonAsync("/api/students", student)
         ↓
    HTTP POST → http://localhost:5000/api/students
         ↓
    Student API → StudentsController.Create()
         ↓
    StudentService.CreateStudentAsync()
         ↓
    StudentRepository.AddAsync()
         ↓
    EF Core → Save to Database
         ↓
    Dapr → Publish "student-created" event
         ↓
    JSON Response ← Created Student (201)
         ↓
    Blazor Server ← Deserialize
         ↓
    Browser ← Navigate to /students
```

### 3. Update Student

```
Browser → EditStudent.razor loads student
         ↓
    StudentApiService.GetStudentByIdAsync(id)
         ↓
    HTTP GET → http://localhost:5000/api/students/{id}
         ↓
    Student API → Return student data
         ↓
    Blazor Server → Pre-populate form
         ↓
    User modifies form → Submit
         ↓
    StudentApiService.UpdateStudentAsync(id, student)
         ↓
    HTTP PUT → http://localhost:5000/api/students/{id}
         ↓
    Student API → Update database
         ↓
    Dapr → Publish "student-updated" event
         ↓
    Browser ← Navigate to /students
```

### 4. Delete Student

```
Browser → User clicks Delete button
         ↓
    StudentApiService.DeleteStudentAsync(id)
         ↓
    HTTP DELETE → http://localhost:5000/api/students/{id}
         ↓
    Student API → Remove from database
         ↓
    Dapr → Publish "student-deleted" event
         ↓
    Response ← 204 No Content
         ↓
    Blazor Server → Refresh student list
         ↓
    Browser ← Updated table
```

---

## Code Integration Points

### 1. Blazor Service Registration (Program.cs)

```csharp
builder.Services.AddHttpClient<StudentApiService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5000");
});
```

### 2. API Service (StudentApiService.cs)

```csharp
public class StudentApiService
{
    private readonly HttpClient _httpClient;

    public StudentApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<Student>> GetAllStudentsAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<Student>>("api/students");
    }
}
```

### 3. Blazor Component (Students.razor)

```razor
@inject StudentApiService StudentService

@code {
    private List<Student>? students;

    protected override async Task OnInitializedAsync()
    {
        students = await StudentService.GetAllStudentsAsync();
    }
}
```

---

## API Endpoints Used

| Method | Endpoint | Blazor Usage | Response |
|--------|----------|--------------|----------|
| GET | `/api/students` | Students.razor | List of students |
| GET | `/api/students/{id}` | EditStudent.razor | Single student |
| POST | `/api/students` | CreateStudent.razor | Created student (201) |
| PUT | `/api/students/{id}` | EditStudent.razor | Updated student (200) |
| DELETE | `/api/students/{id}` | Students.razor | No content (204) |

---

## Data Models

### Blazor Model (Models/Student.cs)

```csharp
public class Student
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int Age { get; set; }
}
```

### API Model (Domain/Entities/Student.cs)

```csharp
public class Student
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int Age { get; set; }
}
```

**Note**: Models are identical for seamless serialization/deserialization.

---

## Error Handling

### Blazor Side

```csharp
public async Task<List<Student>> GetAllStudentsAsync()
{
    try
    {
        return await _httpClient.GetFromJsonAsync<List<Student>>("api/students") 
               ?? new List<Student>();
    }
    catch
    {
        return new List<Student>(); // Return empty list on error
    }
}
```

### Component Side

```razor
@if (students == null || !students.Any())
{
    <div class="alert alert-info">No students found.</div>
}
```

---

## Configuration

### Blazor Configuration (appsettings.json)

```json
{
  "StudentApiUrl": "http://localhost:5000"
}
```

### API Configuration (appsettings.json)

```json
{
  "AllowedHosts": "*"
}
```

**Note**: Add CORS if needed for cross-origin requests.

---

## CORS Configuration (If Needed)

### API Side (Program.cs)

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazor", policy =>
    {
        policy.WithOrigins("https://localhost:7001")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

app.UseCors("AllowBlazor");
```

---

## Testing the Integration

### 1. Start Both Applications

```bash
# Terminal 1 - API
dotnet run --project StudentDaprWithAspire.API

# Terminal 2 - Blazor
dotnet run --project StudentDaprWithAspire.WebBlazor
```

### 2. Verify API is Running

```bash
curl http://localhost:5000/api/students
```

### 3. Open Blazor App

Navigate to: https://localhost:7001/students

### 4. Test CRUD Operations

- Create a student
- View the list
- Edit a student
- Delete a student

---

## Debugging Tips

### 1. Check API Connectivity

```csharp
// Add logging in StudentApiService
private readonly ILogger<StudentApiService> _logger;

public async Task<List<Student>> GetAllStudentsAsync()
{
    _logger.LogInformation("Fetching students from API");
    try
    {
        var students = await _httpClient.GetFromJsonAsync<List<Student>>("api/students");
        _logger.LogInformation($"Fetched {students?.Count ?? 0} students");
        return students ?? new List<Student>();
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error fetching students");
        return new List<Student>();
    }
}
```

### 2. Browser DevTools

- Open F12 Developer Tools
- Check Network tab for API calls
- Check Console for JavaScript errors
- Monitor SignalR connection

### 3. API Logs

Watch API console for incoming requests:
```
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/1.1 GET http://localhost:5000/api/students
```

---

## Performance Optimization

### 1. Caching

```csharp
private List<Student>? _cachedStudents;
private DateTime _cacheTime;

public async Task<List<Student>> GetAllStudentsAsync()
{
    if (_cachedStudents != null && 
        (DateTime.Now - _cacheTime).TotalSeconds < 60)
    {
        return _cachedStudents;
    }

    _cachedStudents = await _httpClient.GetFromJsonAsync<List<Student>>("api/students");
    _cacheTime = DateTime.Now;
    return _cachedStudents ?? new List<Student>();
}
```

### 2. Loading States

```razor
@if (isLoading)
{
    <div class="spinner-border"></div>
}
else
{
    <table>...</table>
}
```

---

## Security Considerations

### 1. HTTPS Only

Ensure both applications use HTTPS in production.

### 2. API Authentication

Add JWT authentication:

```csharp
// API
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => { ... });

// Blazor
builder.Services.AddHttpClient<StudentApiService>(client =>
{
    client.DefaultRequestHeaders.Authorization = 
        new AuthenticationHeaderValue("Bearer", token);
});
```

### 3. Input Validation

Both client and server-side validation:

```razor
<EditForm Model="@student" OnValidSubmit="HandleSubmit">
    <DataAnnotationsValidator />
    <ValidationSummary />
</EditForm>
```

---

## Deployment

### Development

```bash
# API
dotnet run --project StudentDaprWithAspire.API

# Blazor
dotnet run --project StudentDaprWithAspire.WebBlazor
```

### Production

```bash
# API
dotnet publish -c Release
# Deploy to IIS, Azure App Service, or Docker

# Blazor
dotnet publish -c Release
# Deploy to IIS, Azure App Service, or Docker
```

### Docker Compose

```yaml
version: '3.8'
services:
  api:
    build: ./StudentDaprWithAspire.API
    ports:
      - "5000:8080"
  
  blazor:
    build: ./StudentDaprWithAspire.WebBlazor
    ports:
      - "5001:8080"
    environment:
      - StudentApiUrl=http://api:8080
```

---

## Summary

✅ **Blazor Server** communicates with **Student API** via HTTP  
✅ **StudentApiService** encapsulates all API calls  
✅ **HttpClient** configured with base address  
✅ **JSON serialization** automatic with System.Text.Json  
✅ **Error handling** at service and component levels  
✅ **Loading states** for better UX  
✅ **CRUD operations** fully functional  

**The integration is complete and working! 🎉**
