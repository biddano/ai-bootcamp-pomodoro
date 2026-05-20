# Pomodoro App Architecture

## Overview

This application is a simple Pomodoro timer app with a React frontend and a .NET Web API backend.

The backend will use a lightweight vertical slice architecture inspired by Jason Taylor’s Clean Architecture approach, where features are organized around use cases rather than only technical layers.

## React Frontend

*To be defined later.*

## .NET Web API Backend

The backend is responsible for exposing API endpoints that support the Pomodoro timer experience.

Initial backend responsibilities:

* Manage timer session data
* Manage one key task per timer session
* Expose endpoints for creating, reading, updating, and completing timer sessions
* Keep business logic outside of controllers
* Return consistent API responses

## Database for state management
The backend will use SqlLite for storage/state management. 
Initial database resopnsibilities:
* Create a db schema
* Initialize the db tables
* Create the DTO objects based on the db tables in the backend code base.

## Backend Architecture Style

The backend will follow a minimal vertical slice architecture.

Each feature should be organized around a specific use case or business capability. For V1, likely slices include:

Features/
TimerSessions/
CreateTimerSession/
GetTimerSession/
UpdateTimerSession/
CompleteTimerSession/

Each slice may contain the files needed for that feature, such as:

CreateTimerSession/
CreateTimerSessionRequest.cs
CreateTimerSessionResponse.cs
CreateTimerSessionHandler.cs
CreateTimerSessionValidator.cs

## Service Layer

A service layer will contain application-level business logic that should not live directly inside controllers.

Service responsibilities:

* Coordinate timer session behavior
* Validate business rules
* Handle mode-specific duration rules
* Manage focus vs. break timer behavior
* Coordinate persistence when needed

Controllers should delegate business logic to services or feature handlers.

## Project Structure

Proposed initial backend structure:

Pomodoro.Api/
Controllers/
Features/
TimerSessions/
Services/
Models/
Data/
Program.cs
appsettings.json

## Dependency Direction

Dependencies should flow inward toward business logic.

General rules:

* Controllers call services or feature handlers.
* Services contain business logic.
* Data access should not be placed directly in controllers.
* Feature slices should own their request/response models when practical.
* Shared code should only be created when multiple slices genuinely need it.

## V1 Backend Scope

For V1, the backend should support:

* 25-minute focus timer sessions
* 5-minute break timer sessions
* One key task per timer session
* Starting, stopping, resetting, and completing a timer session
* Switching between focus and break modes

## Non-Goals

The following are out of scope for the initial backend architecture:

* Authentication
* User accounts
* Complex authorization
* Analytics
* Notifications
* Background jobs
* Multi-user support
* Advanced reporting
* Overly abstract generic repositories

## Architecture Guidelines

* Keep the architecture simple.
* Do not add abstractions until they are needed.
* Prefer feature-based organization over large generic folders.
* Keep controllers thin.
* Keep business rules testable.
* Avoid placing timer logic directly in API endpoints.
* Keep V1 focused on the basic Pomodoro workflow.
