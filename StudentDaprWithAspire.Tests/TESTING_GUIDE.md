# Testing Documentation - StudentDaprWithAspire

## Test Structure

```
StudentDaprWithAspire.Tests/
├── Controllers/                    # Unit tests for controllers
│   └── StudentsControllerTests.cs
├── Services/                       # Unit tests for services
│   └── StudentServiceTests.cs
├── ContractTests/                  # Contract/API schema tests
│   ├── StudentApiContractTests.cs
│   └── DaprEventContractTests.cs
├── IntegrationTests/              # Integration tests
│   ├── CustomWebApplicationFactory.cs
│   ├── StudentApiIntegrationTests.cs
│   ├── StudentRepositoryIntegrationTests.cs
│   └── DaprPubSubIntegrationTests.cs
└── StudentDaprWithAspire.Tests.csproj
```

---

## Test Types

### 1. Unit Tests
Tests individual components in isolation using mocks.

**Location**: `Controllers/`, `Services/`

**Examples**:
- StudentsControllerTests.cs - Tests controller logic
- StudentServiceTests.cs - Tests service layer logic

**Run**:
```bash
dotnet test --filter "FullyQualifiedName~StudentServiceTests"
```

---

### 2. Contract Tests
Validates API request/response schemas and event structures.

**Location**: `ContractTests/`

**Purpose**:
- Ensure API contracts remain stable
- Validate request/response JSON schemas
- Verify Dapr event structures
- Check data serialization/deserialization

**Test Files**:

#### StudentApiContractTests.cs
- ✅ Student entity has required properties
- ✅ Request/response serialization works correctly
- ✅ Response collections are properly formatted
- ✅ Field validation rules are enforced
- ✅ Response schema matches expected structure

#### DaprEventContractTests.cs
- ✅ Student-created event has correct structure
- ✅ Student-updated event has correct structure
- ✅ Student-deleted event contains required data
- ✅ Topic names follow naming conventions
- ✅ PubSub component name is correct

**Run**:
```bash
dotnet test --filter "FullyQualifiedName~ContractTests"
```

---

### 3. Integration Tests
Tests multiple components working together with real dependencies.

**Location**: `IntegrationTests/`

**Purpose**:
- Test end-to-end API workflows
- Verify database operations
- Validate Dapr integration
- Test real HTTP requests/responses

**Test Files**:

#### StudentApiIntegrationTests.cs
Tests complete API workflows:
- ✅ GET /api/students - Returns all students
- ✅ POST /api/students - Creates new student
- ✅ GET /api/students/{id} - Returns specific student
- ✅ GET /api/students/{id} - Returns 404 for non-existing
- ✅ PUT /api/students/{id} - Updates existing student
- ✅ PUT /api/students/{id} - Returns 400 for ID mismatch
- ✅ DELETE /api/students/{id} - Deletes student
- ✅ DELETE /api/students/{id} - Returns 404 for non-existing
- ✅ Multiple CRUD operations work correctly

#### StudentRepositoryIntegrationTests.cs
Tests repository with actual database:
- ✅ AddAsync adds student to database
- ✅ GetAllAsync returns all students
- ✅ GetByIdAsync returns existing student
- ✅ GetByIdAsync returns null for non-existing
- ✅ UpdateAsync updates student in database
- ✅ UpdateAsync returns null for non-existing
- ✅ DeleteAsync removes student from database
- ✅ DeleteAsync returns false for non-existing
- ✅ Concurrent operations work correctly

#### DaprPubSubIntegrationTests.cs
Tests Dapr event publishing:
- ✅ Publishes student-created event
- ✅ Publishes student-updated event
- ✅ Publishes student-deleted event
- ✅ Uses correct pubsub component name
- ✅ Publishes to correct topics
- ✅ Handles multiple event publishing

**Run**:
```bash
dotnet test --filter "FullyQualifiedName~IntegrationTests"
```

---

## Running Tests

### Run All Tests
```bash
dotnet test
```

### Run Specific Test Category
```bash
# Unit tests only
dotnet test --filter "FullyQualifiedName~Controllers|FullyQualifiedName~Services"

# Contract tests only
dotnet test --filter "FullyQualifiedName~ContractTests"

# Integration tests only
dotnet test --filter "FullyQualifiedName~IntegrationTests"
```

### Run Single Test File
```bash
dotnet test --filter "FullyQualifiedName~StudentApiIntegrationTests"
```

### Run with Code Coverage
```bash
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover
```

### Run with Detailed Output
```bash
dotnet test --logger "console;verbosity=detailed"
```

---

## Test Configuration

### CustomWebApplicationFactory
Custom factory for integration tests that:
- Configures in-memory database
- Replaces production dependencies with test doubles
- Ensures clean test environment

```csharp
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // Configure test services
    }
}
```

---

## Test Dependencies

```xml
<PackageReference Include="xunit" Version="2.9.3" />
<PackageReference Include="xunit.runner.visualstudio" Version="3.1.4" />
<PackageReference Include="Moq" Version="4.20.72" />
<PackageReference Include="FluentAssertions" Version="8.8.0" />
<PackageReference Include="Microsoft.AspNetCore.Mvc.Testing" Version="10.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.InMemory" Version="10.0.0" />
<PackageReference Include="Dapr.Client" Version="1.15.0" />
<PackageReference Include="coverlet.collector" Version="6.0.4" />
```

---

## Best Practices

### 1. Test Naming Convention
```
MethodName_Scenario_ExpectedBehavior
```
Example: `GetStudentById_ExistingStudent_ReturnsStudent`

### 2. Arrange-Act-Assert Pattern
```csharp
[Fact]
public async Task Example()
{
    // Arrange - Setup test data
    var student = new Student { ... };
    
    // Act - Execute the operation
    var result = await _service.CreateAsync(student);
    
    // Assert - Verify the outcome
    result.Should().NotBeNull();
}
```

### 3. Use FluentAssertions
```csharp
// Instead of
Assert.Equal(expected, actual);

// Use
actual.Should().Be(expected);
```

### 4. Isolate Tests
- Each test should be independent
- Use in-memory database with unique names
- Clean up resources in Dispose()

### 5. Mock External Dependencies
- Mock DaprClient for unit tests
- Use real dependencies for integration tests

---

## Test Coverage Goals

| Layer | Target Coverage |
|-------|----------------|
| Controllers | 90%+ |
| Services | 95%+ |
| Repositories | 90%+ |
| Domain Entities | 80%+ |
| Overall | 85%+ |

---

## Continuous Integration

### GitHub Actions Workflow
```yaml
- name: Run Tests
  run: dotnet test --no-build --verbosity normal
  
- name: Generate Coverage Report
  run: dotnet test /p:CollectCoverage=true
```

---

## Contract Testing Benefits

1. **API Stability**: Ensures API contracts don't break
2. **Documentation**: Tests serve as living documentation
3. **Consumer Confidence**: Consumers can trust the API structure
4. **Early Detection**: Catches breaking changes before deployment
5. **Microservices**: Essential for service-to-service communication

---

## Integration Testing Benefits

1. **Real Scenarios**: Tests actual user workflows
2. **Database Validation**: Verifies data persistence
3. **End-to-End**: Tests complete request/response cycle
4. **Confidence**: Higher confidence in production readiness
5. **Bug Detection**: Catches integration issues early

---

## Troubleshooting

### Tests Fail with Database Errors
- Ensure unique database names for parallel tests
- Check DbContext disposal in test cleanup

### Dapr Tests Fail
- Verify Dapr client mocking is correct
- Check event topic names match

### Integration Tests Timeout
- Increase test timeout in test settings
- Check for deadlocks in async code

---

## Next Steps

1. Add performance tests
2. Add load tests with k6 or JMeter
3. Add mutation testing with Stryker.NET
4. Add API contract testing with Pact
5. Add E2E tests with Playwright
6. Implement test data builders
7. Add snapshot testing for responses

---

## Resources

- [xUnit Documentation](https://xunit.net/)
- [Moq Documentation](https://github.com/moq/moq4)
- [FluentAssertions](https://fluentassertions.com/)
- [ASP.NET Core Testing](https://docs.microsoft.com/aspnet/core/test/)
- [Dapr Testing](https://docs.dapr.io/developing-applications/sdks/dotnet/)
