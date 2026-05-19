# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

Pomodoro Timer is a focus timer application with a .NET Web API backend and React frontend (frontend in separate repo). The backend uses ASP.NET Core with a vertical slice architecture pattern for feature organization.

**Key documentation:**
- `docs/architecture-doc.md` — Backend architecture and design rationale
- `docs/requirements-doc.md` — V1 feature scope and user flow

## Backend Architecture

The backend follows a minimal vertical slice architecture where features are organized around specific use cases rather than by technical layers.

### Feature Slice Structure

Each feature lives under `Features/<FeatureName>/` and owns all artifacts needed for that feature:

```
Features/
  TimerSessions/
    CreateTimerSession/
      CreateTimerSessionRequest.cs
      CreateTimerSessionResponse.cs
      CreateTimerSessionHandler.cs
      CreateTimerSessionValidator.cs
    GetTimerSession/
    UpdateTimerSession/
    CompleteTimerSession/
```

New features should follow this pattern. Do not create shared abstractions until needed by multiple slices.

### Dependency Direction

- Controllers delegate to services or feature handlers; they do not contain business logic.
- Services contain application-level business logic and validation.
- Feature slices own their request/response models.
- Data access should not live in controllers.

Dependencies flow inward toward business logic, never outward to controllers.

### V1 Scope

The backend supports: 25-minute focus sessions, 5-minute break sessions, one key task per session, and session state management (start, stop, reset, complete, mode switching). Non-goals: authentication, user accounts, authorization, analytics, notifications, background jobs.

## Build and Run

**Build the project:**
```powershell
dotnet build Pomodoro.Api/Pomodoro.Api.csproj
```

**Run the API locally (development):**
```powershell
dotnet run --project Pomodoro.Api/Pomodoro.Api.csproj --launch-profile http
```

The API will be available at `http://localhost:5176` (HTTP profile) or `https://localhost:7195` (HTTPS profile). OpenAPI/Swagger documentation is at `/openapi/v1.json` in development.

**Run a single file as a test** (when tests are added):
```powershell
dotnet test Pomodoro.Api.Tests/Pomodoro.Api.Tests.csproj --filter ClassName.MethodName
```

## Code Patterns and Standards

Use the project skill `/dotnet-api-patterns` when creating or modifying:
- API endpoints and controllers
- Services and business logic
- DTOs, request/response models
- Validation logic
- Dependency injection configuration
- Backend tests

The dotnet-api-patterns skill ensures consistency with the architecture defined in `docs/architecture-doc.md`.

## Feature Implementation Workflow

Use the `/feature-implementation-workflow` skill when:
- Adding a new Pomodoro feature
- Updating existing timer behavior
- Creating or modifying API endpoints
- Fixing bugs that affect user-facing behavior

This skill guides you through understanding scope, identifying affected systems (frontend only, backend only, or both), and checking for applicable coding standards before implementation.

## Important Notes

- The `WeatherForecastController.cs` and `WeatherForecast.cs` are scaffolding and should be removed.
- Keep controllers thin; move business logic to services or feature handlers.
- Don't create shared abstractions until at least two slices genuinely need them.
- All feature handlers should have request/response models and validation.
- The architecture is intentionally minimal for V1; complexity should only be added when genuinely needed.
