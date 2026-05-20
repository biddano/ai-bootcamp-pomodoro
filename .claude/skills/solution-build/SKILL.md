---
name: solution-build
description: Compile the .NET backend solution in ./src/backend
---

Builds the .NET backend solution. Runs `dotnet build ./src/backend`.

---
name: build-frontend
description: Build the React frontend application in ./src/frontend
---

Builds the React frontend. Installs dependencies with npm and runs `npm run build` in ./src/frontend.

---
name: build-all
description: Build both the backend (.NET) and frontend (React) applications
---

Builds the complete PomoTimer solution. First builds the backend, then the frontend. Both builds must succeed.
