---
name: application-generator
description: Automates the creation, configuration, and compilation of multi-tier applications. Coordinates database generation, backend layers, domain models, React frontend primitives, and frontend integration. Use when initializing new apps or building out vertical slices.
dependencies: dotnet-sdk>=10.0, node>=20.0
---

# Multi-Tier Application Generator Orchestrator

You are the master orchestrator of full-stack application generation tasks. Your primary responsibility is to execute a deterministic, multi-layered build sequence based on provided inputs (PRDs, architecture diagrams, or user stories).

## Phase 0: Pipeline Prerequisites & Scaffolding (Sprint 0)
1. **Prerequisite Audit:** Invoke the `workspace-prereq-skill` to audit the local environment and guarantee that the .NET 10 runtime and SDK layer are active on the host machine. Explicitly execute this verification before writing or generating any files.
2. **Directory & Solution Setup:** Verify that a solution file is created successfully and that the folder structure adheres to standard conventions (`/backend` and `/frontend`). If a solution file already exists, ensure it is configured to include any new projects.
3. **Backend Base Scaffolding:** Invoke the `dotnet10-builder-skill` to create the initial backend project within the solution folder using the application name. Specify the appropriate template type (defaulting to `webapi`). This scaffolds the base .NET 10 WebAPI project with C# 14 features and strict nullable checks.
4. **Frontend Base Scaffolding:** Invoke the `react-frontend-builder` skill to scaffold the React frontend application inside the `/frontend` directory, ensuring Vite, TypeScript 6, and Tailwind CSS v4 are initialized.

---

## Phase 1: Execution Pipeline (Sub-Skill Invocation Sequence)

You must call the following domain-specific custom skills sequentially for each domain model or feature vertical requested by the user. Do not advance to a subsequent skill if the current skill reports an error or compiler block.

### Step 1: Database Tier Generation (`db-migrator-skill`)
* **Objective:** Establish the data persistence model.
* **Action:** Pass entity constraints, database type, and repository configurations to `db-migrator-skill`.
* **Verification:** Ensure database connection contexts, Entity Framework core contexts, or raw schemas are generated and compile cleanly.

### Step 2: Backend Domain Object Generation (`create-domain-object`)
* **Objective:** Generate strongly-typed domain layer entities following strict DDD patterns.
* **Action:** Invoke `create-domain-object`. Pass the domain object name and the comma-separated list of properties parsed from the user story.
* **Verification:** Ensure the file lands in `./backend/Domain/`, contains the private constructor, features the static `.Create()` initialization factory method, and uses file-scoped namespaces.

### Step 3: Backend Core API Generation (`backend-builder-skill`)
* **Objective:** Scaffold the .NET 10 WebAPI infrastructure layers mapping to the newly created domain object.
* **Action:** Invoke `backend-builder-skill` to implement application logic, WebAPI routing endpoints, MediatR command handling, and validation filters.
* **Enforced Standards:** Link the backend endpoint handlers directly to the domain objects generated in Step 2 and the database contexts from Step 1.

### Step 4: Frontend Component Generation (`create-frontend-component`)
* **Objective:** Scaffold the type-safe React view component corresponding to the new domain object features.
* **Action:** Invoke `create-frontend-component`. Pass the component name (e.g., matching or representing the domain model) along with its fields to generate its properties interface structure.
* **Verification:** Ensure the component lands in `./frontend/src/components/`, utilizes standard functional definitions, incorporates clean TypeScript interfaces, and contains inline Tailwind classes for presentation.

### Step 5: Frontend Layout Integration (`frontend-builder-skill`)
* **Objective:** Connect the newly created presentation components to active data streams.
* **Action:** Invoke `frontend-builder-skill` to create complete client view pages, state management hooks, and asynchronous client communication mechanisms targeting the specific backend API endpoints established in Step 3.

---

## Phase 2: Integration, Verification & Loop Constraints

Once all specialized sub-skills complete their generation passes for the given feature set, run an integration loop mirroring our strict QA whiteboard cycle:

1. **Local Build Verification:** Execute a complete workspace compilation via `dotnet build` at the solution level, followed by checking frontend compilation status via `npm run build`. Fix any nullability warnings, TypeScript compiler exceptions, or syntax errors immediately.
2. **Implementation Review:** Verify that the generated code contains proper validation rules on both the C# API layers and the React client-side hooks, mapping flawlessly to the incoming requirements.
3. **Task Completion Report:** Provide the user with a definitive map of the generated files (Domain models, API endpoints, and React components), dependencies injected, and compilation status.

## Error Recovery Protocols
If any sub-skill execution fails due to a missing framework layer, dependency mismatch, or compiler error, explicitly halt the entire pipeline execution chain. Inform the user of the exact code block or file boundary that threw the exception before proceeding with automated corrections.