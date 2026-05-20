---
name: react-frontend-builder
description: Automates the creation, configuration, and execution of React client single-page applications. Use when initializing frontend frames, configuring build systems (Vite), adding component architectures, or installing client packages.
dependencies: node>=20.0, npm>=10.0
---

# React Frontend Application Builder Specification

## Core Actions

### 0. Build the Workspace Solution
If there is no existing frontend architecture root, create a clean directory at the root of the workspace named `./frontend/`. If an application frame exists, target that directory explicitly for all downstream file mutations, package installs, and compilation verifications.

### 1. Project Initialization
When asked to create a new UI module, dashboard, or client app layout:
* Run the initialization scaffolding command using Vite with the TypeScript variant:
  ```bash
  npm create vite@latest . -- --template react-ts