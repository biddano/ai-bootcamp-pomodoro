---
name: commit-code
description: For committing code to the local branch.
  Create a git commit with auto-generated message describing the changes.
---

Creates a commit on the current branch with a brief description of the changes.

**Validation checks:**
- Verifies current branch is NOT main (stops with warning if it is)
- Verifies there are outstanding changes to commit (stops with warning if there aren't any)

**Process:**
1. Checks current branch and uncommitted changes
2. Reviews staged and unstaged changes to understand what was modified
3. Generates a concise commit message briefly describing the changes
4. Executes `scripts/create-commit.ps1` with the generated commit message to create the commit

**After commit:**
- Informs you the commit was created successfully
- Reminds you that you must manually push to origin with `git push`
- Does NOT automatically push
