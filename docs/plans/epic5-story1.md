# Epic 5, Story 1: Create Timer Session with Mode and Task

## Epic Information

**Epic Number:** EPIC-05  
**Epic Title:** Backend API and Service Layer

## Story Information

**Story Number:** STORY-05-01  
**Story Title:** Create Timer Session with Mode and Task  
**Story Points:** 5  
**User Value:** As an API consumer, I want to create a new timer session with a mode and task so that the frontend can initiate a Pomodoro session

## Technical Context

This story implements the first critical endpoint for the Pomodoro backend. It establishes the foundational patterns that subsequent stories will build on: domain models with mode/state enums, the service layer, the vertical slice architecture for the TimerSessions feature, and the API response format. The implementation must apply mode-specific duration rules (Focus=1500s, Break=300s) in the service layer, not the controller. The domain model will use a relational database with Entity Framework Core for persistence.

**Key Architecture Decisions:**
- Vertical slice: Each operation (Create, Get, Update, Complete) owns its request/response models and handler
- Service layer: TimerSessionService applies business rules and duration logic; controllers remain thin
- Domain model: TimerSession entity uses Guid for id, enums for Mode and State, DateTime for timestamps
- Database: Entity Framework Core with migrations; in-memory provider for tests
- API Response: All endpoints return consistent wrapper format (success, data, statusCode)

## Task List

### Phase 1: Domain Model Foundation

1. **Create TimerSession domain entity** (Feature slice: Models)
   - File: `src/backend/Pomodoro.Api/Models/TimerSession.cs`
   - Properties: id (Guid), mode (Mode enum), state (State enum), remainingTime (int), task (string, nullable, max 200), createdAt (DateTime), completedAt (DateTime?), lastUpdatedAt (DateTime)
   - Use primary constructor or explicit properties; ensure all timestamps are DateTime (UTC)

2. **Create Mode enum**
   - File: `src/backend/Pomodoro.Api/Models/Mode.cs`
   - Values: Focus (0), Break (1)
   - This enum determines the session duration

3. **Create State enum**
   - File: `src/backend/Pomodoro.Api/Models/State.cs`
   - Values: NotStarted (0), Running (1), Paused (2), Completed (3)

### Phase 2: Data Access and Persistence

4. **Create Entity Framework DbContext**
   - File: `src/backend/Pomodoro.Api/Data/PomodoroDbContext.cs`
   - Configure TimerSession DbSet
   - Entity configuration: primary key (Id), value conversion for enums, constraints for task max length
   - Use UTC DateTimeKind for all timestamp properties

5. **Create initial database migration**
   - Command: `dotnet ef migrations add InitialCreate`
   - Generates migration file with TimerSession table schema
   - Ensures id (Guid primary key), mode (int enum), state (int enum), remainingTime (int), task (nvarchar(200), nullable), timestamps
   - Target: LocalDb or test database connection string in appsettings.json

6. **Configure dependency injection for DbContext**
   - File: `src/backend/Pomodoro.Api/Program.cs`
   - Register PomodoroDbContext in DI container with appropriate lifetime
   - Add connection string to appsettings.json (use LocalDb for development)

### Phase 3: Request/Response Models and Validation

7. **Create CreateTimerSessionRequest DTO**
   - File: `src/backend/Pomodoro.Api/Features/TimerSessions/CreateTimerSession/CreateTimerSessionRequest.cs`
   - Properties: mode (Mode enum), task (string, optional, max 200 characters)
   - Slice-owned; not shared with other features

8. **Create CreateTimerSessionResponse DTO**
   - File: `src/backend/Pomodoro.Api/Features/TimerSessions/CreateTimerSession/CreateTimerSessionResponse.cs`
   - Properties: id (Guid), mode (Mode enum), state (State enum), remainingTime (int), task (string), createdAt (DateTime, ISO 8601), lastUpdatedAt (DateTime, ISO 8601)
   - Ensure timestamps are serialized as ISO 8601 UTC strings

9. **Create CreateTimerSessionValidator**
   - File: `src/backend/Pomodoro.Api/Features/TimerSessions/CreateTimerSession/CreateTimerSessionValidator.cs`
   - Validate: mode is valid enum value, task is null or max 200 characters
   - Return validation errors if invalid

### Phase 4: Service Layer Business Logic

10. **Create TimerSessionService**
    - File: `src/backend/Pomodoro.Api/Services/TimerSessionService.cs`
    - Method: `CreateSessionAsync(Mode mode, string? task) => Task<TimerSession>`
    - Logic: Set remainingTime based on mode (Focus=1500, Break=300), set state=NotStarted, set createdAt/lastUpdatedAt to UtcNow
    - Save to database and return the created entity
    - Register in DI container (Program.cs)
    - This is the application-level business logic layer

### Phase 5: Controller and Request Handling

11. **Create TimerSessionsController**
    - File: `src/backend/Pomodoro.Api/Controllers/TimerSessionsController.cs`
    - Thin endpoint: POST /api/timer-sessions
    - Receive CreateTimerSessionRequest, validate, delegate to service, return CreateTimerSessionResponse with HTTP 201
    - Return consistent ApiResponse wrapper: `{success: true, data: {...}, statusCode: 201}`

12. **Create CreateTimerSessionHandler (optional alternative pattern)**
    - File: `src/backend/Pomodoro.Api/Features/TimerSessions/CreateTimerSession/CreateTimerSessionHandler.cs`
    - Encapsulates request handling logic separate from controller
    - Controller can delegate to this handler if following MediatR-style pattern
    - If using MediatR, register handler in DI; otherwise, handler can be called directly from controller

### Phase 6: API Response Format Infrastructure

13. **Create ApiResponse<T> wrapper class**
    - File: `src/backend/Pomodoro.Api/Models/ApiResponse.cs`
    - Generic class: success (bool), data (T), statusCode (int)
    - Ensure all controllers use this wrapper for consistency

14. **Create ApiError class (if not already present)**
    - File: `src/backend/Pomodoro.Api/Models/ApiError.cs`
    - Properties: message (string), code (string), details (object?)
    - Used for error responses in later stories

### Phase 7: Testing

15. **Unit tests for CreateTimerSessionValidator**
    - File: `src/backend/Pomodoro.Api.Tests/Features/TimerSessions/CreateTimerSession/CreateTimerSessionValidatorTests.cs`
    - Test: Valid mode, valid task
    - Test: Invalid mode
    - Test: Task exceeds 200 characters
    - Test: Null/empty task is accepted

16. **Unit tests for TimerSessionService CreateSessionAsync**
    - File: `src/backend/Pomodoro.Api.Tests/Services/TimerSessionServiceTests.cs`
    - Test: Create focus session sets remainingTime=1500
    - Test: Create break session sets remainingTime=300
    - Test: Initial state is NotStarted
    - Test: createdAt and lastUpdatedAt are set
    - Test: Returned entity has valid Guid id

17. **Integration tests for CreateTimerSession endpoint**
    - File: `src/backend/Pomodoro.Api.Tests/Features/TimerSessions/CreateTimerSession/CreateTimerSessionIntegrationTests.cs`
    - Test: POST /api/timer-sessions with focus mode and task returns 201 with correct fields
    - Test: POST /api/timer-sessions with break mode and empty task returns 201
    - Test: Session created is immediately queryable (integration with GetTimerSession endpoint)
    - Test: Timestamps are ISO 8601 format
    - Use in-memory database or test database for isolation

## Testing Plan

### Unit Tests

**CreateTimerSessionValidator Tests:**
- Valid input (mode and task): passes validation
- Invalid mode value: returns validation error
- Task > 200 characters: returns validation error
- Null/empty task: passes validation

**TimerSessionService CreateSessionAsync Tests:**
- Mode Focus sets remainingTime to 1500 seconds
- Mode Break sets remainingTime to 300 seconds
- State is set to NotStarted
- createdAt is set to UtcNow
- lastUpdatedAt is set to createdAt
- Returns entity with valid Guid id
- Task property is preserved (or null if not provided)

### Integration Tests

**CreateTimerSession Endpoint Tests:**
- Scenario: Create focus session with task='Complete report'
  - POST /api/timer-sessions with {mode: Focus, task: 'Complete report'}
  - Verify HTTP 201 response
  - Verify response contains: id (non-empty Guid), mode=Focus, state=NotStarted, remainingTime=1500, task='Complete report', createdAt, lastUpdatedAt
  - Verify timestamps are ISO 8601 UTC format

- Scenario: Create break session without task
  - POST /api/timer-sessions with {mode: Break, task: null}
  - Verify HTTP 201 response
  - Verify remainingTime=300, task is empty string or null

- Scenario: Session is immediately queryable
  - Create session with POST
  - Extract id from response
  - Call GET /api/timer-sessions/{id}
  - Verify full session object is returned with matching data

- Scenario: Validation errors return proper format
  - POST /api/timer-sessions with invalid mode
  - Verify HTTP 400 response
  - Verify error response contains message and code

## Definition of Done

- [ ] **Domain Models Created**
  - [ ] TimerSession.cs entity with all required properties
  - [ ] Mode enum with Focus and Break values
  - [ ] State enum with NotStarted, Running, Paused, Completed values
  - [ ] All timestamps use DateTime UTC

- [ ] **Database Setup**
  - [ ] PomodoroDbContext created and configured
  - [ ] Migration created and applied to test/local database
  - [ ] Connection string configured in appsettings.json

- [ ] **DTOs and Request/Response Models**
  - [ ] CreateTimerSessionRequest.cs with mode and task properties
  - [ ] CreateTimerSessionResponse.cs with complete session fields
  - [ ] ApiResponse<T> wrapper created
  - [ ] Timestamps serialized as ISO 8601 UTC strings

- [ ] **Business Logic Layer**
  - [ ] TimerSessionService created with CreateSessionAsync method
  - [ ] Mode-specific duration logic implemented (Focus=1500, Break=300)
  - [ ] Service registered in DI container
  - [ ] No business logic in controllers

- [ ] **API Endpoint**
  - [ ] POST /api/timer-sessions endpoint implemented in TimerSessionsController
  - [ ] Endpoint receives CreateTimerSessionRequest and returns CreateTimerSessionResponse
  - [ ] HTTP 201 (Created) status returned on success
  - [ ] Response wrapped in ApiResponse format: {success: true, data: {...}, statusCode: 201}
  - [ ] Timestamp fields in ISO 8601 format

- [ ] **Input Validation**
  - [ ] CreateTimerSessionValidator enforces mode validity
  - [ ] Task property validated for max 200 characters
  - [ ] Null/empty task accepted for break sessions
  - [ ] Validation errors return HTTP 400 with error details

- [ ] **Testing**
  - [ ] Unit tests for CreateTimerSessionValidator pass (4+ test cases)
  - [ ] Unit tests for TimerSessionService pass (6+ test cases)
  - [ ] Integration tests for endpoint pass (4+ test cases)
  - [ ] All tests use appropriate isolation (in-memory DB for integration tests)
  - [ ] Code coverage for handler and service methods > 85%

- [ ] **Code Quality**
  - [ ] No hardcoded credentials or secrets in code or config
  - [ ] Error messages are meaningful and user-facing
  - [ ] Logging in place for business logic (optional for V1 but recommended)
  - [ ] Code follows existing style guide and naming conventions
  - [ ] No unnecessary comments; only non-obvious WHY documented

- [ ] **Documentation**
  - [ ] Code compiles without warnings
  - [ ] Migration applied cleanly without errors
  - [ ] Feature slice directory structure follows architecture doc

## Acceptance Criteria Coverage

| Acceptance Criterion | Task(s) | Test Case(s) |
|---|---|---|
| Create focus session with task | Tasks 10, 11 | Integration: Create focus session with task |
| Create break session without task | Tasks 10, 11 | Integration: Create break session without task |
| Session immediately queryable | Tasks 4, 5, 10, 11 | Integration: Session is immediately queryable |
| HTTP 201 returned | Task 11 | Integration: Verify HTTP 201 status |
| Session object in response | Tasks 8, 11 | Integration: Response contains id, mode, duration, task, createdAt |

---

**Implementation Notes:**
- Dependencies flow inward: Controller → Service → Repository/DbContext → Database
- Keep controllers thin; TimerSessionService contains duration logic
- Use test database or in-memory EF Core provider for integration tests to avoid polluting dev database
- Ensure UTC timestamps throughout (use DateTime.UtcNow)
- ISO 8601 format for timestamp serialization in responses (JSON serializer default)
