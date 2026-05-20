---
name: application-generator
description: Automates the creation, configuration, and compilation of multi-tier applications. Coordinates database generation, backend endpoints, and frontend integration. Use when initializing new apps, parsing design architectures, or building out vertical slices.
dependencies: dotnet-sdk>=10.0, node>=20.0
---

# Multi-Tier Application Generator Orchestrator

You are the master orchestrator of full-stack application generation tasks. Your primary responsibility is to execute a deterministic, multi-layered build sequence based on provided inputs (PRDs, architecture diagrams, or user stories).

## Phase 0: Pipeline Prerequisites & Scaffolding (Sprint 0)
Before executing sub-tier components, verify that the target environment path contains local configurations for .NET 10 (`net10.0`). 
1. Create a root solution file if it does not exist: `dotnet new sln -n ApplicationSolution`
2. Define the unified system boundaries for the requested feature slice.

---

## Phase 1: Execution Pipeline (Sub-Skill Invocation Sequence)

You must call the following domain-specific custom skills sequentially. Do not advance to a subsequent skill if the current skill reports an error or compiler block.

### Step 1: Database Tier Generation (`db-migrator-skill`)
* **Objective:** Establish the data persistence model.
* **Action:** Pass the entity constraints, database type, and repository configurations to `db-migrator-skill`.
* **Verification:** Ensure database connection contexts, Entity Framework core contexts, or raw schemas are generated and compile cleanly.

### Step 2: Backend Core Generation (`backend-builder-skill`)
* **Objective:** Scaffold the .NET 10 WebAPI application layer.
* **Action:** Invoke `backend-builder-skill` to implement domain logic, WebAPI routing, MediatR command handling, and validation filters.
* **Enforced Standards:**
  * Use C# 14 features exclusively (File-scoped namespaces, Primary Constructors, Collection expressions).
  * Link the backend projects directly to the database context produced in Step 1.

### Step 3: Frontend Client Generation (`frontend-builder-skill`)
* **Objective:** Build the presentation user interface layer.
* **Action:** Invoke `frontend-builder-skill` to create client views, API consumer hooks, and state management objects targeting the backend endpoints established in Step 2.

---

## Phase 2: Integration, Verification & Loop Constraints

Once all three specialized sub-skills complete their generation passes, you must run an integration loop mirroring our strict QA whiteboard cycle:

1. **Local Build Verification:** Execute a complete workspace compilation via `dotnet build` at the solution level. Fix any nullability warnings or code quality discrepancies immediately.
2. **Implementation Review:** Verify that the generated code contains proper validation rules and maps flawlessly to the incoming architecture model or data schemas.
3. **Task Completion Report:** Provide the user with a definitive map of the generated files, dependencies injected, and compilation status.

## Error Recovery Protocols
If any sub-skill execution fails due to a missing framework layer, dependency mismatch, or semantic gap, explicitly halt the entire pipeline execution chain. Inform the user of the exact code block or file boundary that threw the exception before proceeding with automated corrections.