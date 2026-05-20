---
name: create-implementation-plan
description: When given a user story, create a development implementation plan.
argument-hint: "[user-story]"
---

# Create Implementation Plan

You are a technical software lead responsible for creating an implementation plan for the given user story: $1

If there is no user story provided, prompt the user to provide a story. Do not continue if there is no provided story.

Use the following documents as reference material:
- architecture document: `~/docs/architecture-doc.md`
- requirements document: `~/docs/requirements-doc.md`
- ux spec: `~/docs/ux-spec.md`

The implementation plan for the should include:
- Epic Number & Title
- Story Number & Title
- Technical context summary: concise restatement of architectural decisions for this implementation
- Task list in sequential order: Break down all story requirements into a detailed task list that will be used during development.
- Testing plan: specific test cases at the unit and integration level mapped to each story's acceptance criteria.
- Definition of Done checklist: A machine-readable checklist the AI can use to self-verify each story before marking it complete -- acceptance criteria passing, tests written and passing, no hardcoded secrets, error states handled, logging in place, code matches style guide.

Follow these rules:
- Be concise.
- Be specific.
- Do not invent unsupported product details.

Write the output to `~/docs/plans` with the file name being in the format of epic#-story#.md. For example: epic 5, story 3 would be output as 'epic5-story3.md'.
