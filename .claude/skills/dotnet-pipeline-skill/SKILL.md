---
name: dotnet-pipeline-skill
description: Main execution pipeline for managing C# .NET 10 tasks. Run this whenever initializing or compiling the application to ensure environment readiness.
dependencies: bash
---

# .NET 10 Project Pipeline

When the user asks to create, build, or manage a C# application, you must execute the following multi-step pipeline sequence exactly in order.

## Execution Sequence

### Step 1: Environment Verification
Invoke the `workspace-prereq-skill` to audit the local environment and guarantee that the .NET 10 runtime and SDK layer are active on the host machine. 
* *Instruction:* Explicitly execute the prerequisite verification before writing or generating any files.

### Step 2: Code Scaffolding and Compilation
Once Step 1 confirms a successful exit status, transition the context to the `dotnet10-builder-skill`.
* *Instruction:* Pass the user's specific project generation, framework template, or compilation requirements cleanly into the builder skill payload instructions.
