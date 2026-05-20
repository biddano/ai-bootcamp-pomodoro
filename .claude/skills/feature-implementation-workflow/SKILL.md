---
name: feature-implementation-workflow
description: Use this skill when implementing a new feature, modifying an existing feature, or making coordinated frontend/backend changes. We are following clean architecture principles with a focus on feature-based organization and clear dependency direction. This skill will guide you through the process of making changes while adhering to our architectural guidelines and best practices.
---

# Feature Implementation Workflow

## Instructions

Reference this requirements document: `requirements-doc.md`

Use when: 
- Adding a new feature
- Updating existing behavior
- Creating or modifying API endpoints
- Connecting frontend components to backend data
- Refactoring feature-related code
- Fixing bugs that affect user-facing behavior

Before making code changes:
1. Understand the requested behavior. Pause to clarify any uncertainties with the requester or stakeholders.
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