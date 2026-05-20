---
name: create-pr
description: Create a draft pull request using GitHub CLI with auto-generated description
---

Creates a draft pull request on GitHub for the current branch.

**Validation checks:**
- Verifies current branch is NOT main (stops with warning if it is)
- Verifies there are no uncommitted changes (stops with warning if there are any)
- Use the solution-build skill to build everything.  (stops with warning if any build fails)
- Use the unit-tester skill to run all the unit tests. (stops with warning if any fail)

**Process:**
1. Validates branch and working directory state
2. Retrieves commit messages on the current branch since the last PR
3. Generates a PR description from those commit messages
4. Executes `scripts/create-draft-pr.ps1` with the generated title and description to create the draft PR
5. Does NOT publish the PR

**After PR creation:**
- Provides the PR URL and draft status
- Reminds you to review the PR description and details
- Instructs you to publish the PR when ready (via GitHub UI or `gh pr ready`)
- Does NOT automatically publish
