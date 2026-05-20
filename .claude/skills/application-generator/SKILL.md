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

## Phase 1: Feature Implementation (Sub-Skill Invocation Sequence)

You must call the following skills sequentially for each feature vertical or user story. Parse the story or plan to derive domain object names, properties, and component fields before invoking each skill. Do not advance to a subsequent step if the current skill reports an error or compiler block.

### Step 1: Backend Domain Object (`create-domain-object`)
* **Objective:** Generate the strongly-typed domain entity for the feature.
* **Action:** Invoke `create-domain-object`. Pass the domain object name and comma-separated property list parsed from the story or plan.
* **Verification:** Ensure the file lands in `./backend/Domain/`, has a private constructor, exposes a static `.Create()` factory method, and uses file-scoped namespaces.

### Step 2: Backend API Layer (`dotnet-api-patterns`)
* **Objective:** Scaffold the WebAPI feature slice mapping to the domain object from Step 1.
* **Action:** Invoke `dotnet-api-patterns` to generate the vertical slice — request/response models, handler, validator, and endpoint registration — following the project's architecture standards.
* **Verification:** Ensure the slice compiles cleanly via `solution-build` before proceeding.

### Step 3: Backend Unit Tests
* **Objective:** Write xUnit unit tests covering the domain object and API handler created in Steps 1–2.
* **Action:** Create test classes in `./backend` test project. Cover the `.Create()` factory method, validation rules, and key handler behaviors derived directly from the acceptance criteria in the story or plan.
* **Verification:** All new tests must compile. Do not run them yet — that happens in Phase 2.

### Step 4: Frontend Component (`create-frontend-component`)
* **Objective:** Scaffold the React presentation component for the feature.
* **Action:** Invoke `create-frontend-component`. Pass the component name and fields matching the domain model to generate TypeScript interfaces and Tailwind-styled JSX.
* **Verification:** Ensure the component lands in `./frontend/src/components/` and compiles cleanly.

### Step 5: Frontend Integration
* **Objective:** Connect the new component to live data from the backend API.
* **Action:** Create the necessary state management hooks and async API client calls targeting the endpoints established in Step 2. Wire the component into the relevant page or layout.
* **Verification:** Confirm `npm run build` succeeds with no TypeScript errors before proceeding.

---

## Phase 2: Test & Commit

Once all Phase 1 implementation steps are complete, execute the following in strict order:

1. **Run Tests:** Invoke `unit-tester` to execute the full .NET xUnit test suite. If any tests fail, fix the failures and re-run until the suite is fully green. Do not proceed to the commit step until all tests pass.
2. **Commit:** Invoke `commit-code` to commit all verified changes with an auto-generated message describing the feature. The commit must only be created after a green test run.
3. **Task Completion Report:** Provide the user with a summary of generated files (domain objects, API slices, test classes, React components), skills invoked, and final compilation and test status.

## Error Recovery Protocols
If any sub-skill execution fails due to a missing framework layer, dependency mismatch, or compiler error, explicitly halt the entire pipeline execution chain. Inform the user of the exact code block or file boundary that threw the exception before proceeding with automated corrections.