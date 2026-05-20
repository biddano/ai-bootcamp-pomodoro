---
name: unit-tester
description: Run all .NET xUnit tests in ./src/backend
---

Runs all xUnit tests in the .NET backend solution. Executes `dotnet test ./src/backend`.

---
name: test-backend-project
description: Run xUnit tests for a specific .NET project in ./src/backend
---

Runs xUnit tests for a specific project. Provide the project name or path within ./src/backend.

---
name: test-frontend
description: Run unit tests for the React frontend in ./src/frontend
---

Runs unit tests for the React frontend. Executes `npm test` in ./src/frontend.

---
name: test-all
description: Run all unit tests for both backend (.NET) and frontend (React)
---

Runs the complete test suite. First runs all .NET xUnit tests in ./src/backend, then runs React unit tests in ./src/frontend. Both test runs must succeed.
