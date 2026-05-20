# Epic 5, Story 3: Pause and Resume Timer Session

## Epic Information

**Epic Number:** EPIC-05  
**Epic Title:** Backend API and Service Layer

## Story Information

**Story Number:** STORY-05-03  
**Story Title:** Pause and Resume Timer Session  
**Story Points:** 3  
**User Value:** As an API consumer, I want to pause and resume a timer session so that the frontend can control the timer workflow

## Technical Context

This story implements pause and resume functionality for the UpdateTimerSession endpoint. It builds on the domain models and service layer established in earlier stories (Create, Get). The implementation must enforce strict state transition rules: only Running sessions can be paused, only Paused sessions can be resumed, and the timer's remaining time must be preserved across pause/resume cycles. All state changes must be persisted to the database and return HTTP 200 with the updated session object.

**Key Architecture Decisions:**
- State transitions validated in TimerSessionService business logic, not in controller
- Valid transitions: Running → Paused, Paused → Running; invalid transitions return HTTP 400
- remainingTime is immutable during pause/resume; only state changes
- UpdateTimerSession endpoint is shared across multiple action types (Pause, Resume, Reset, ModeSwitch, TaskUpdate); each action delegates to appropriate service method
- Vertical slice maintains request/response models for this feature
- Database updates use Entity Framework Core SaveChangesAsync

## Task List

### Phase 1: Request/Response Models

1. **Create or extend UpdateTimerSessionRequest DTO**
   - File: `src/backend/Pomodoro.Api/Features/TimerSessions/UpdateTimerSession/UpdateTimerSessionRequest.cs`
   - Properties: sessionId (Guid), action (string enum: "Pause", "Resume", "Reset", "SwitchMode", "UpdateTask"), actionDetails (object, nullable for extensibility)
   - For Pause and Resume: action field is sufficient; no additional actionDetails needed
   - Slice-owned; not shared

2. **Create or extend UpdateTimerSessionResponse DTO**
   - File: `src/backend/Pomodoro.Api/Features/TimerSessions/UpdateTimerSession/UpdateTimerSessionResponse.cs`
   - Properties: id (Guid), mode (Mode enum), state (State enum), remainingTime (int), task (string), lastUpdatedAt (DateTime, ISO 8601)
   - Same shape as CreateTimerSessionResponse for consistency
   - Timestamps serialized as ISO 8601 UTC strings

3. **Create UpdateTimerSessionValidator (if not already present)**
   - File: `src/backend/Pomodoro.Api/Features/TimerSessions/UpdateTimerSession/UpdateTimerSessionValidator.cs`
   - Validate: sessionId is valid (non-empty Guid), action is valid enum value
   - For Pause/Resume: no additional validation needed at request level; state transition validation happens in service

### Phase 2: Service Layer Business Logic

4. **Extend TimerSessionService with state transition validation**
   - File: `src/backend/Pomodoro.Api/Services/TimerSessionService.cs`
   - Add method: `PauseSessionAsync(Guid sessionId) => Task<TimerSession>`
     - Fetch session by id; if not found, throw NotFoundException
     - If state is not Running, throw ValidationException with message "Cannot pause a session that is not running"
     - Set state to Paused, update lastUpdatedAt to UtcNow
     - Save to database and return updated session
   
   - Add method: `ResumeSessionAsync(Guid sessionId) => Task<TimerSession>`
     - Fetch session by id; if not found, throw NotFoundException
     - If state is not Paused, throw ValidationException with message "Cannot resume a session that is not paused"
     - Set state to Running, update lastUpdatedAt to UtcNow
     - Save to database and return updated session
   
   - Define custom exceptions: SessionNotFoundException, InvalidStateTransitionException (or ValidationException)
   - Exceptions should have meaningful messages for HTTP error responses

5. **Create or extend UpdateTimerSessionHandler**
   - File: `src/backend/Pomodoro.Api/Features/TimerSessions/UpdateTimerSession/UpdateTimerSessionHandler.cs`
   - Handler receives UpdateTimerSessionRequest
   - Route to appropriate service method based on action ("Pause" → PauseSessionAsync, "Resume" → ResumeSessionAsync)
   - Catch service exceptions and translate to appropriate HTTP responses (400 for validation, 404 for not found)
   - Return UpdateTimerSessionResponse with updated session data

### Phase 3: Controller Endpoint

6. **Create or extend UpdateTimerSessionController endpoint**
   - File: `src/backend/Pomodoro.Api/Controllers/TimerSessionsController.cs`
   - Endpoint: PUT /api/timer-sessions/{sessionId}
   - Receive UpdateTimerSessionRequest in body (or query/route params)
   - Validate request; delegate to UpdateTimerSessionHandler
   - Return HTTP 200 with UpdateTimerSessionResponse wrapped in ApiResponse format on success
   - Return HTTP 400 with error details on validation failure (invalid state transition)
   - Return HTTP 404 with error details if session not found
   - Response format: `{success: true, data: {...}, statusCode: 200}` on success; `{success: false, error: {message, code}, statusCode: 400/404}` on error

### Phase 4: Error Handling Infrastructure

7. **Define custom exceptions (if not already present)**
   - File: `src/backend/Pomodoro.Api/Exceptions/SessionNotFoundException.cs`
   - File: `src/backend/Pomodoro.Api/Exceptions/InvalidStateTransitionException.cs`
   - Exceptions should include meaningful error messages
   - Include an error code property (e.g., "SESSION_NOT_FOUND", "INVALID_STATE_TRANSITION")

8. **Create or extend global exception handler (if not already present)**
   - File: `src/backend/Pomodoro.Api/Middleware/ExceptionHandlingMiddleware.cs` or use exception filter
   - Handle SessionNotFoundException → HTTP 404
   - Handle InvalidStateTransitionException → HTTP 400
   - Ensure error responses follow ApiError format: {message, code}
   - Ensure consistent error response wrapping in ApiResponse

### Phase 5: Testing

9. **Unit tests for PauseSessionAsync**
   - File: `src/backend/Pomodoro.Api.Tests/Services/TimerSessionServiceTests.cs` (add to existing)
   - Test: Pause a Running session → state becomes Paused, remainingTime unchanged, lastUpdatedAt updated
   - Test: Pause a Paused session → throws InvalidStateTransitionException with appropriate message
   - Test: Pause a NotStarted session → throws InvalidStateTransitionException
   - Test: Pause a Completed session → throws InvalidStateTransitionException
   - Test: Pause non-existent session → throws SessionNotFoundException

10. **Unit tests for ResumeSessionAsync**
    - File: `src/backend/Pomodoro.Api.Tests/Services/TimerSessionServiceTests.cs` (add to existing)
    - Test: Resume a Paused session → state becomes Running, remainingTime unchanged, lastUpdatedAt updated
    - Test: Resume a Running session → throws InvalidStateTransitionException
    - Test: Resume a NotStarted session → throws InvalidStateTransitionException
    - Test: Resume a Completed session → throws InvalidStateTransitionException
    - Test: Resume non-existent session → throws SessionNotFoundException

11. **Integration tests for Pause/Resume workflow**
    - File: `src/backend/Pomodoro.Api.Tests/Features/TimerSessions/UpdateTimerSession/PauseResumeIntegrationTests.cs`
    - Test: Create session → transition to Running → call Pause → verify state=Paused and remainingTime preserved
    - Test: Paused session → call Resume → verify state=Running and remainingTime unchanged
    - Test: Call Pause on Paused session → HTTP 400 with validation error message
    - Test: Call Resume on Running session → HTTP 400 with validation error message
    - Test: Call Pause on non-existent session → HTTP 404 with not found error
    - Test: Pause/Resume cycle preserves remainingTime across multiple cycles
    - Verify HTTP 200 response on success, error response format on failure

### Phase 6: Documentation and Code Review

12. **Update architectural documentation (if needed)**
    - Verify CLAUDE.md reflects pause/resume state transition rules
    - Ensure feature slice pattern is followed

13. **Code review and quality assurance**
    - Ensure no hardcoded values; all state names and transitions use defined enums
    - Verify error messages are clear and actionable
    - Confirm timestamps use UTC and ISO 8601 format in responses
    - Check for logging of important business events (optional for V1)

## Testing Plan

### Unit Tests

**PauseSessionAsync Tests:**
- Pause Running session: state transitions to Paused, remainingTime preserved, lastUpdatedAt updated
- Pause Paused session: throws InvalidStateTransitionException
- Pause NotStarted session: throws InvalidStateTransitionException
- Pause Completed session: throws InvalidStateTransitionException
- Pause non-existent session: throws SessionNotFoundException with session id in error

**ResumeSessionAsync Tests:**
- Resume Paused session: state transitions to Running, remainingTime preserved, lastUpdatedAt updated
- Resume Running session: throws InvalidStateTransitionException
- Resume NotStarted session: throws InvalidStateTransitionException
- Resume Completed session: throws InvalidStateTransitionException
- Resume non-existent session: throws SessionNotFoundException

### Integration Tests

**Pause/Resume Workflow Tests:**
- Scenario: Pause a running timer
  - Create session (state=NotStarted)
  - Manually set state to Running (or via future Start endpoint)
  - Call PUT /api/timer-sessions/{id} with action=Pause
  - Verify HTTP 200 response
  - Verify state=Paused, remainingTime unchanged
  - Verify lastUpdatedAt updated to current time
  - Verify response includes all session fields

- Scenario: Resume a paused timer
  - Create session with state=Running (via earlier workflow)
  - Call PUT with action=Pause
  - Call PUT with action=Resume
  - Verify HTTP 200 response
  - Verify state=Running, remainingTime unchanged
  - Verify lastUpdatedAt updated

- Scenario: Cannot pause already paused session
  - Create session → Pause → Pause again
  - Verify HTTP 400 response
  - Verify error message contains: "Cannot pause a session that is not running" or similar
  - Verify error code: "INVALID_STATE_TRANSITION" or "CANNOT_PAUSE_PAUSED_SESSION"

- Scenario: Cannot resume non-paused session
  - Create session in NotStarted state
  - Call PUT with action=Resume
  - Verify HTTP 400 response with appropriate error message

- Scenario: Session not found
  - Call PUT /api/timer-sessions/{non-existent-id} with action=Pause
  - Verify HTTP 404 response
  - Verify error message contains session id

- Scenario: Pause/Resume cycle preserves time
  - Create session with remainingTime=1000
  - Pause, verify remainingTime=1000
  - Resume, verify remainingTime=1000 still

## Definition of Done

- [ ] **Request/Response Models**
  - [ ] UpdateTimerSessionRequest.cs supports action field (Pause, Resume, etc.)
  - [ ] UpdateTimerSessionResponse.cs with complete session fields
  - [ ] Timestamps serialized as ISO 8601 UTC

- [ ] **Service Layer Business Logic**
  - [ ] PauseSessionAsync method implemented with state transition validation
  - [ ] ResumeSessionAsync method implemented with state transition validation
  - [ ] remainingTime preserved during state transitions
  - [ ] lastUpdatedAt updated on state change
  - [ ] Custom exceptions defined: SessionNotFoundException, InvalidStateTransitionException
  - [ ] Exception messages are clear and include relevant context (e.g., current state)

- [ ] **Database Persistence**
  - [ ] State transitions persisted via Entity Framework SaveChangesAsync
  - [ ] lastUpdatedAt updated automatically on save
  - [ ] No dirty state or failed transactions

- [ ] **API Endpoint**
  - [ ] PUT /api/timer-sessions/{sessionId} endpoint implemented
  - [ ] Endpoint receives UpdateTimerSessionRequest with action field
  - [ ] HTTP 200 returned on successful pause/resume
  - [ ] Response wrapped in ApiResponse format: {success: true, data: {...}, statusCode: 200}
  - [ ] HTTP 400 returned for invalid state transitions with error details
  - [ ] HTTP 404 returned for non-existent session
  - [ ] Error responses include message and error code

- [ ] **Error Handling**
  - [ ] Global exception handler translates custom exceptions to HTTP responses
  - [ ] SessionNotFoundException → HTTP 404
  - [ ] InvalidStateTransitionException → HTTP 400
  - [ ] Error messages are user-facing and actionable
  - [ ] No stack traces exposed in production responses

- [ ] **Testing**
  - [ ] Unit tests for PauseSessionAsync pass (5+ test cases)
  - [ ] Unit tests for ResumeSessionAsync pass (5+ test cases)
  - [ ] Integration tests for Pause/Resume endpoint pass (6+ test cases)
  - [ ] All tests use appropriate isolation (in-memory DB for integration tests)
  - [ ] Code coverage for pause/resume logic > 85%
  - [ ] Tests verify state transitions, error messages, and HTTP status codes

- [ ] **Code Quality**
  - [ ] No hardcoded credentials or secrets
  - [ ] State transition logic uses defined enums, not magic strings
  - [ ] Code follows existing style guide and naming conventions
  - [ ] No unnecessary comments; WHY documented only when non-obvious
  - [ ] Code compiles without warnings

- [ ] **Documentation**
  - [ ] Feature implementation follows vertical slice pattern
  - [ ] Architecture documentation updated if state transition rules changed

## Acceptance Criteria Coverage

| Acceptance Criterion | Task(s) | Test Case(s) |
|---|---|---|
| Pause a running timer | Tasks 4, 6 | Unit: Pause Running session; Integration: Pause running timer |
| Resume a paused timer | Tasks 4, 6 | Unit: Resume Paused session; Integration: Resume paused timer |
| Cannot pause already paused | Tasks 4, 6 | Unit: Pause Paused session; Integration: Cannot pause already paused |
| HTTP 200 on success | Task 6 | Integration: All success scenarios return 200 |
| HTTP 400 on invalid transition | Task 6, 8 | Integration: Cannot pause paused, cannot resume running |
| remainingTime preserved | Tasks 4, 11 | Unit: Pause/Resume preserve time; Integration: Cycle preserves time |
| State persisted to database | Task 4 | Integration: State change visible on subsequent GET |

---

**Implementation Notes:**
- State machine: NotStarted ↔ Running ↔ Paused; only these two transitions are valid
  - Running → Paused: allowed
  - Paused → Running: allowed
  - Any other transition: invalid (throw exception)
- Use DateTime.UtcNow for lastUpdatedAt updates
- remainingTime is immutable during pause/resume; do not decrement during pause operations
- Leverage existing DbContext from story 1; no new database changes needed
- If UpdateTimerSessionHandler doesn't exist yet, create it following the pattern from CreateTimerSessionHandler
- Exception handling can be added as middleware or as a controller exception filter; choose one pattern for consistency
- Consider logging state transitions for debugging (optional for V1)
