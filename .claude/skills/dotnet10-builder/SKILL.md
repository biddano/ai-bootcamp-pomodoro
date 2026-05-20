---
name: dotnet10-builder-skill
description: Automates the creation, configuration, and compilation of C# applications targeting .NET 10 (net10.0). Use when initializing new apps, managing dependency packages, or executing builds.
dependencies: dotnet-sdk >= 10.0
---

# .NET 10 Application Builder Specification

## Core Actions

### 0. Build the solution
If there is no existing solution file, create one with the name from the user prompt based on the application name at the root of the workspace in `./backend/` using the command `dotnet new sln -n <ApplicationName>`. If a solution file already exists, ensure it is properly configured to include any new projects that will be generated in subsequent steps.

### 1. Project Initialization
When asked to create a new application or module:
* Run the specific command: `dotnet new <template_type> -f net10.0` (Templates include `console`, `webapi`, `blazor`, `classlib`).
* If no template type is provided, default to `webapi`.
* Immediately update the generated `.csproj` file to explicitly include modern C# 14 features and strict nullable checks:
  ```xml
  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    <LangVersion>preview</LangVersion>
    <AnalysisLevel>latest-recommended</AnalysisLevel>
  </PropertyGroup>
### 2. 
