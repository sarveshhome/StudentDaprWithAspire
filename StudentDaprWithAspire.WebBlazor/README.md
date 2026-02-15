# StudentDaprWithAspire.WebBlazor

Blazor Server Web Application for Student Management System

## Overview

This is a Blazor Server application that provides a web UI for managing students. It consumes the Student API and provides full CRUD functionality through an interactive web interface.

## Features

- ✅ **Interactive UI** - Blazor Server with real-time updates
- ✅ **CRUD Operations** - Create, Read, Update, Delete students
- ✅ **Responsive Design** - Bootstrap 5 responsive layout
- ✅ **API Integration** - Consumes Student REST API
- ✅ **Form Validation** - Client-side validation
- ✅ **Loading States** - User-friendly loading indicators
- ✅ **Error Handling** - Graceful error messages

## Project Structure

```
StudentDaprWithAspire.WebBlazor/
├── Components/
│   ├── Layout/
│   │   ├── MainLayout.razor       # Main layout
│   │   └── NavMenu.razor          # Navigation menu
│   └── Pages/
│       ├── Home.razor             # Home page
│       ├── Students.razor         # Student list page
│       ├── CreateStudent.razor    # Create student form
│       └── EditStudent.razor      # Edit student form
├── Models/
│   └── Student.cs                 # Student model
├── Services/
│   └── StudentApiService.cs      # HTTP client service
├── wwwroot/                       # Static files
├── appsettings.json              # Configuration
└── Program.cs                     # Application entry point
```

## Pages

### 1. Home (`/`)
- Welcome page with system overview
- Quick navigation to student management
- Feature highlights

### 2. Students List (`/students`)
- Display all students in a table
- Search and filter capabilities
- Quick actions: Edit, Delete
- Add new student button
- Refresh data button

### 3. Create Student (`/students/create`)
- Form to create new student
- Fields: Name, Email, Age
- Client-side validation
- Success/error feedback

### 4. Edit Student (`/students/edit/{id}`)
- Form to update existing student
- Pre-populated with current data
- Client-side validation
- Success/error feedback

## Configuration

### appsettings.json

```json
{
  "StudentApiUrl": "http://localhost:5000"
}
```

### Environment Variables

You can override the API URL using environment variables:

```bash
export StudentApiUrl="http://localhost:5000"
```

## Running the Application

### Prerequisites

- .NET 10.0 SDK
- Student API running on http://localhost:5000

### Run Standalone

```bash
dotnet run --project StudentDaprWithAspire.WebBlazor
```

Application will be available at:
- HTTP: http://localhost:5001
- HTTPS: https://localhost:7001

### Run with Aspire

```bash
dotnet run --project StudentDaprWithAspire.AppHost
```

Access via Aspire Dashboard at http://localhost:15000

## Development

### Add New Page

1. Create new `.razor` file in `Components/Pages/`
2. Add `@page` directive with route
3. Add navigation link in `NavMenu.razor`

Example:
```razor
@page "/mypage"

<PageTitle>My Page</PageTitle>

<h1>My Page</h1>
```

### Add New Service

1. Create service class in `Services/`
2. Register in `Program.cs`:

```csharp
builder.Services.AddScoped<MyService>();
```

## API Integration

### StudentApiService

The `StudentApiService` provides methods to interact with the Student API:

```csharp
// Get all students
var students = await StudentService.GetAllStudentsAsync();

// Get student by ID
var student = await StudentService.GetStudentByIdAsync(id);

// Create student
var created = await StudentService.CreateStudentAsync(student);

// Update student
var updated = await StudentService.UpdateStudentAsync(id, student);

// Delete student
var success = await StudentService.DeleteStudentAsync(id);
```

## UI Components

### Bootstrap Icons

The application uses Bootstrap Icons:

- `bi-people-fill` - Students
- `bi-plus-circle` - Add
- `bi-pencil` - Edit
- `bi-trash` - Delete
- `bi-arrow-clockwise` - Refresh
- `bi-save` - Save
- `bi-x-circle` - Cancel

### Styling

- Bootstrap 5 for responsive layout
- Custom CSS in `wwwroot/app.css`
- Component-specific styles in `.razor.css` files

## Render Modes

This application uses **Interactive Server** render mode:

```razor
@rendermode InteractiveServer
```

Benefits:
- Real-time UI updates
- Server-side state management
- SignalR connection for interactivity

## Error Handling

The application handles errors gracefully:

- **API Unavailable**: Shows empty state with message
- **Network Errors**: Displays error alerts
- **Not Found**: Shows 404 page
- **Validation Errors**: Inline form validation

## Performance

### Optimization Tips

1. **Lazy Loading**: Load data only when needed
2. **Caching**: Cache frequently accessed data
3. **Pagination**: Implement pagination for large datasets
4. **Debouncing**: Debounce search inputs

## Security

### Best Practices

- ✅ HTTPS enabled by default
- ✅ Antiforgery tokens for forms
- ✅ Input validation
- ✅ CORS configuration
- ⚠️ Add authentication/authorization as needed

## Testing

### Manual Testing

1. Start Student API
2. Start Blazor app
3. Navigate to `/students`
4. Test CRUD operations

### Automated Testing

Add bUnit tests for components:

```bash
dotnet add package bUnit
```

## Deployment

### Publish

```bash
dotnet publish -c Release -o ./publish
```

### Docker

Create `Dockerfile`:

```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY ./publish .
ENTRYPOINT ["dotnet", "StudentDaprWithAspire.WebBlazor.dll"]
```

Build and run:

```bash
docker build -t student-blazor .
docker run -p 8080:8080 student-blazor
```

## Troubleshooting

### API Connection Issues

**Problem**: Cannot connect to Student API

**Solution**:
- Verify API is running on http://localhost:5000
- Check `StudentApiUrl` in appsettings.json
- Verify CORS settings in API

### SignalR Connection Errors

**Problem**: Blazor Server connection drops

**Solution**:
- Check network connectivity
- Increase SignalR timeout
- Review browser console for errors

### Page Not Updating

**Problem**: UI not reflecting changes

**Solution**:
- Call `StateHasChanged()` after async operations
- Verify `@rendermode InteractiveServer` is set
- Check for exceptions in browser console

## Future Enhancements

- [ ] Add authentication (Identity, Azure AD)
- [ ] Implement search and filtering
- [ ] Add pagination for large datasets
- [ ] Export to Excel/PDF
- [ ] Real-time notifications with SignalR
- [ ] Dark mode theme
- [ ] Localization support
- [ ] Audit logging
- [ ] Advanced validation rules
- [ ] File upload for student photos

## Resources

- [Blazor Documentation](https://docs.microsoft.com/aspnet/core/blazor/)
- [Bootstrap 5](https://getbootstrap.com/)
- [Bootstrap Icons](https://icons.getbootstrap.com/)
- [SignalR](https://docs.microsoft.com/aspnet/core/signalr/)

## Support

For issues or questions:
- Check API is running and accessible
- Review browser console for errors
- Verify configuration in appsettings.json
