---
name: feature-implementation-workflow
description: Use this skill when implementing a new feature, modifying an existing feature, or making coordinated frontend/backend changes
---

# Feature Implementation Workflow

## Instructions

Reference this requirements document: `requirements-doc.md`

Use when: 
- Adding a new Pomodoro feature
- Updating existing timer behavior
- Creating or modifying API endpoints
- Connecting frontend components to backend data
- Refactoring feature-related code
- Fixing bugs that affect user-facing behavior

Before making code changes:

1. Understand the requested behavior.
2. Identify whether the change affects:
   - Frontend only
   - Backend only
   - Both frontend and backend
   - Shared API contracts
3. Review the existing code patterns before introducing new ones.
4. Identify the smallest set of files needed for the change.
5. Check whether another project skill applies, such as:
   - `react-ui-patterns`
   - `dotnet-api-patterns`
   - `timer-domain-logic`
   - `api-contract-sync`
   - `testing-standards`