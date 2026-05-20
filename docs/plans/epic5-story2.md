# Epic 5, Story 2: Retrieve Timer Session by ID

## Epic Information

**Epic Number:** EPIC-05  
**Epic Title:** Backend API and Service Layer

## Story Information

**Story Number:** STORY-05-02  
**Story Title:** Retrieve Timer Session by ID  
**Story Points:** 3  
**User Value:** As an API consumer, I want to retrieve a timer session by ID so that the frontend can fetch the current session state

## Technical Context

This story implements the read endpoint, building directly on Story 1's foundation. The domain model, database layer, and API response format are already established. This story focuses on adding a GET endpoint that queries the database by session ID and handles the not-found case with HTTP 404. The service layer will perform the database lookup; the controller remains thin. This pattern (thin controller delegating to service) sets the template for all remaining CRUD operations.

**Key Architecture Decisions:**
- Service layer: TimerSessionService.GetSessionAsync(Guid id) encapsulates the database query
- Controller: Thin GET endpoint that delegates to service and handles null result
- Error handling: Return 404 with structured error response when session not found
- Response format: Existing ApiResponse<T> wrapper reused for consistency
- No new abstractions: Use DbContext directly in service (no separate repository layer for V1)

## Task List

### Phase 1: Add Service Layer Method

1. **Add GetSessionAsync method to TimerSessionService**
   - File: `src/backend/Pomodoro.Api/Services/TimerSessionService.cs`
   - Method signature: `GetSessionAsync(Guid id) => Task<TimerSession?>`
   - Implementation: Query DbContext.TimerSessions.FirstOrDefaultAsync(s => s.Id == id)
   - Return TimerSession if found, null if not found
   - No filtering or transformation; return entity as-is

### Phase 2: Controller Endpoint

2. **Add GetTimerSession endpoint to TimerSessionsController**
   - File: `src/backend/Pomodoro.Api/Controllers/TimerSessionsController.cs`
   - Endpoint: GET /api/timer-sessions/{id}
   - Parameter: id (Guid) from route
   - Logic:
     - Call TimerSessionService.GetSessionAsync(id)
     - If result is not null: return 200 with session object in ApiResponse wrapper
     - If result is null: return 404 with error message
   - Ensure endpoint uses [HttpGet("{id}")] attribute

3. **Create GetTimerSessionResponse DTO**
   - File: `src/backend/Pomodoro.Api/Features/TimerSessions/GetTimerSession/GetTimerSessionResponse.cs`
   - Properties: id (Guid), mode (Mode enum), state (State enum), remainingTime (int), task (string), createdAt (DateTime, ISO 8601), lastUpdatedAt (DateTime, ISO 8601)
   - Can mirror CreateTimerSessionResponse or reuse if structure is identical
   - Ensure timestamps are ISO 8601 UTC format

### Phase 3: Error Handling

4. **Implement not-found error response**
   - Update ApiError class (if needed) to support proper error format
   - Controller returns: HTTP 404 with body: `{success: false, error: {message: "Timer session not found", code: "SESSION_NOT_FOUND"}, statusCode: 404}`
   - Consistent with Story 1's error handling pattern

### Phase 4: Testing

5. **Unit test for TimerSessionService.GetSessionAsync**
   - File: `src/backend/Pomodoro.Api.Tests/Services/TimerSessionServiceTests.cs`
   - Test: Retrieve existing session returns session object
   - Test: Retrieve non-existent session returns null
   - Mock DbContext or use in-memory provider

6. **Integration tests for GetTimerSession endpoint**
   - File: `src/backend/Pomodoro.Api.Tests/Features/TimerSessions/GetTimerSession/GetTimerSessionIntegrationTests.cs`
   - Test: GET /api/timer-sessions/{id} with existing session returns 200 with correct session data
   - Test: GET /api/timer-sessions/{id} with non-existent id returns 404 with error message
   - Test: Returned session object has all expected fields (id, mode, state, remainingTime, task, createdAt, lastUpdatedAt)
   - Test: Timestamps are ISO 8601 UTC format
   - Use in-memory database; create test session before assertion

7. **Integration test: Cross-story flow (Story 1 → Story 2)**
   - Create session via POST /api/timer-sessions (Story 1)
   - Retrieve same session via GET /api/timer-sessions/{id} (Story 2)
   - Verify data integrity and consistency

## Testing Plan

### Unit Tests

**TimerSessionService.GetSessionAsync Tests:**
- Retrieve existing session: returns session object with matching id
- Retrieve non-existent session: returns null
- Query uses correct id parameter
- No unintended data modifications

### Integration Tests

**GetTimerSession Endpoint Tests:**
- Scenario: Retrieve existing session
  - Setup: Create session via POST /api/timer-sessions (from Story 1)
  - Action: GET /api/timer-sessions/{sessionId}
  - Verify: HTTP 200 response
  - Verify: Response contains {success: true, data: {...sessionObject...}, statusCode: 200}
  - Verify: Session object includes: id, mode, state, remainingTime, task, createdAt, lastUpdatedAt
  - Verify: Timestamps are ISO 8601 UTC format (e.g., "2026-05-20T14:30:00Z")
  - Verify: data.id matches requested session id

- Scenario: Retrieve non-existent session
  - Action: GET /api/timer-sessions/{invalid-guid}
  - Verify: HTTP 404 response
  - Verify: Response contains {success: false, error: {message: "...", code: "..."}, statusCode: 404}
  - Verify: No null reference exceptions

- Scenario: Invalid Guid format in route
  - Action: GET /api/timer-sessions/not-a-guid
  - Verify: HTTP 400 (bad request) or ASP.NET Core's default Guid binding error handling applies

## Definition of Done

- [ ] **Service Layer**
  - [ ] GetSessionAsync method added to TimerSessionService
  - [ ] Method accepts Guid id parameter
  - [ ] Method returns Task<TimerSession?> (nullable)
  - [ ] Query uses DbContext.TimerSessions.FirstOrDefaultAsync
  - [ ] Method handles null result gracefully (returns null, not throwing)

- [ ] **API Endpoint**
  - [ ] GET /api/timer-sessions/{id} endpoint implemented
  - [ ] [HttpGet("{id}")] attribute correctly applied
  - [ ] Route parameter binding for Guid id works correctly
  - [ ] Controller calls TimerSessionService.GetSessionAsync

- [ ] **Response Handling**
  - [ ] HTTP 200 returned when session is found
  - [ ] Response body wrapped in ApiResponse<GetTimerSessionResponse>
  - [ ] Response includes: {success: true, data: {...}, statusCode: 200}
  - [ ] Response data includes: id, mode, state, remainingTime, task, createdAt, lastUpdatedAt
  - [ ] Timestamps serialized as ISO 8601 UTC strings

- [ ] **Error Handling**
  - [ ] HTTP 404 returned when session not found
  - [ ] Error response includes ApiError with message and code
  - [ ] Error message: "Timer session not found" (or similar, user-facing)
  - [ ] Error code: "SESSION_NOT_FOUND"
  - [ ] No stack traces or internal errors exposed to client

- [ ] **Input Validation**
  - [ ] Guid route parameter parsed correctly
  - [ ] Invalid Guid format handled (ASP.NET Core default or custom handling)

- [ ] **Testing**
  - [ ] Unit test for GetSessionAsync with existing session passes
  - [ ] Unit test for GetSessionAsync with non-existent session passes
  - [ ] Integration test for 200 response with valid session passes
  - [ ] Integration test for 404 response with invalid session id passes
  - [ ] Integration test for timestamp format (ISO 8601) passes
  - [ ] Cross-story test (Create then Retrieve) passes

- [ ] **Code Quality**
  - [ ] No hardcoded ids or test data in production code
  - [ ] Error messages are clear and user-facing
  - [ ] No unnecessary comments
  - [ ] Code follows naming conventions (GetSessionAsync, GetTimerSessionResponse, etc.)
  - [ ] Code compiles without warnings

- [ ] **Documentation**
  - [ ] GetTimerSession feature slice directory exists: Features/TimerSessions/GetTimerSession/
  - [ ] All files organized within feature slice (response DTO, tests)

## Acceptance Criteria Coverage

| Acceptance Criterion | Task(s) | Test Case(s) |
|---|---|---|
| Retrieve existing session returns 200 with session object | Tasks 1, 2 | Integration: Retrieve existing session |
| Retrieve non-existent session returns 404 with error message | Tasks 2, 4 | Integration: Retrieve non-existent session |
| Response includes all session fields (mode, state, remainingTime, task, timestamps) | Tasks 1, 2, 3 | Integration: Verify response structure |
| HTTP status codes correct (200, 404) | Task 2 | Integration tests validate status codes |

## Dependencies

- **Story 1 (STORY-05-01)** — Domain model, database setup, and ApiResponse format must be complete
- TimerSessionService must exist with GetSessionAsync method
- ApiResponse<T> wrapper must support both success and error responses

## Implementation Notes

- **Service pattern:** Keep business logic in TimerSessionService; controller just receives result and formats response
- **Null handling:** Service returns null for not-found; controller checks null and returns 404 (don't throw)
- **No filtering:** Return session as stored; don't filter or transform data at this layer
- **Error response consistency:** Match error format used in Story 1 (if Story 1 validation errors have a format, use same)
- **Async/await:** Use async queries (FirstOrDefaultAsync) to avoid blocking threads
- **Timestamps:** Ensure stored timestamps are already UTC; serialize with JSON serializer default (ISO 8601)

---

**Estimated Implementation Time:** 1-2 hours (assuming Story 1 complete)
