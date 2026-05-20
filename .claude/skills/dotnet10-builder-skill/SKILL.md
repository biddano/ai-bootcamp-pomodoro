---
name: dotnet10-builder-skill
description: Automates the creation, configuration, and compilation of C# applications targeting .NET 10 (net10.0). Use when initializing new apps, managing dependency packages, or executing builds.
dependencies: dotnet-sdk >= 10.0
---

# .NET 10 Application Builder Specification

## Core Actions

### 0. Build the solution

When asked to create a new application or module:
* Check if the dotnet-sk is installed.
* If no dotnet-sdk. Install the latest version.
* Then run the Project Initialization

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
