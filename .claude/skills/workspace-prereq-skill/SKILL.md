---
name: workspace-prereq-skill
description: Verifies local workspace environment dependencies and automatically installs missing components. Run this before building or initializing projects.
dependencies: bash
disable-model-invocation: true
---

# Workspace Environment Pre-requisites

## Environment Verification Workflow
Before compiling or modifying applications, you must ensure the correct framework layers are active.

1. **Check Environment:** Instantly execute the local automated script helper:
   ```bash
   ./scripts/ensure_dotnet10.sh
