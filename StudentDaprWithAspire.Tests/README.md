# StudentDaprWithAspire - Test Suite

## Quick Start

```bash
# Run all tests
dotnet test

# Run with coverage
dotnet test /p:CollectCoverage=true

# Run specific category
dotnet test --filter "FullyQualifiedName~ContractTests"
dotnet test --filter "FullyQualifiedName~IntegrationTests"
```

---

## Test Structure

```
Tests/
├── Controllers/           → Unit Tests (Controller Layer)
├── Services/             → Unit Tests (Service Layer)
├── ContractTests/        → API Contract & Schema Tests
└── IntegrationTests/     → End-to-End Integration Tests
```

---

## Test Categories

### 🔹 Unit Tests (Existing)
- **StudentsControllerTests** - Controller logic with mocked dependencies
- **StudentServiceTests** - Service layer business logic

### 🔹 Contract Tests (New)
- **StudentApiContractTests** - API request/response schemas
- **DaprEventContractTests** - Event structure validation

### 🔹 Integration Tests (New)
- **StudentApiIntegrationTests** - Full API workflow tests
- **StudentRepositoryIntegrationTests** - Database operations
- **DaprPubSubIntegrationTests** - Event publishing tests

---

## Test Count Summary

| Category | Test Files | Test Cases |
|----------|-----------|------------|
| Unit Tests | 2 | ~20 |
| Contract Tests | 2 | ~12 |
| Integration Tests | 3 | ~25 |
| **Total** | **7** | **~57** |

---

## Key Features

✅ **Contract Testing** - Validates API schemas and event structures  
✅ **Integration Testing** - Tests complete workflows with real dependencies  
✅ **In-Memory Database** - Fast, isolated database tests  
✅ **Dapr Mocking** - Tests event publishing without Dapr runtime  
✅ **FluentAssertions** - Readable, expressive test assertions  
✅ **WebApplicationFactory** - Real HTTP testing  

---

## Example Test Run

```bash
$ dotnet test

Starting test execution, please wait...
A total of 1 test files matched the specified pattern.

Passed!  - Failed:     0, Passed:    57, Skipped:     0, Total:    57
```

---

## Documentation

📖 **[TESTING_GUIDE.md](./TESTING_GUIDE.md)** - Comprehensive testing documentation

Includes:
- Detailed test descriptions
- Running specific tests
- Best practices
- Troubleshooting
- CI/CD integration

---

## Dependencies

```xml
<PackageReference Include="xunit" />
<PackageReference Include="Moq" />
<PackageReference Include="FluentAssertions" />
<PackageReference Include="Microsoft.AspNetCore.Mvc.Testing" />
<PackageReference Include="Microsoft.EntityFrameworkCore.InMemory" />
<PackageReference Include="Dapr.Client" />
```

---

## Test Examples

### Contract Test
```csharp
[Fact]
public void Student_ShouldHaveRequiredProperties()
{
    var student = new Student { Id = 1, Name = "John", Email = "john@test.com", Age = 20 };
    
    student.Should().NotBeNull();
    student.Id.Should().Be(1);
}
```

### Integration Test
```csharp
[Fact]
public async Task CreateStudent_ReturnsCreatedStudent()
{
    var response = await _client.PostAsJsonAsync("/api/students", newStudent);
    
    response.StatusCode.Should().Be(HttpStatusCode.Created);
}
```

---

## CI/CD Integration

Tests run automatically on:
- ✅ Pull requests
- ✅ Commits to main branch
- ✅ Release builds

See `.github/workflows/ci.yml` for configuration.

---

## Coverage Goals

- Controllers: **90%+**
- Services: **95%+**
- Repositories: **90%+**
- Overall: **85%+**

---

## Contributing

When adding new features:
1. Write contract tests for API changes
2. Write integration tests for workflows
3. Write unit tests for business logic
4. Ensure all tests pass before PR

---

## Support

For issues or questions:
- Check [TESTING_GUIDE.md](./TESTING_GUIDE.md)
- Review existing test examples
- Run tests with `--logger "console;verbosity=detailed"`
