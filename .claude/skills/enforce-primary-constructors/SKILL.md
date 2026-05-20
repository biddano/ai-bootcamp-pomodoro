---
name: enforce-primary-constructors
description: When creating/editing C# objects, enforce the use of primary constructors in classes
---

When creating or editing C# classes, this skill enforces the use of primary constructors.

If a class is created or modified and it does not have a primary constructor, and the class contains a constructor with parameters, the skill will:
- Detect the absence of a primary constructor in the class definition

Primary constructor parameters will always be assigned to a readonly field with the same name with leading underscore.

**Example output:**
```csharp
namespace Test.Sample;

public class SampleService(ISampleRepo sampleRepo) : ISampleService
{
    private readonly ISampleRepo _sampleRepo = sampleRepo;

    public void SampleAction() 
    {
      //Do sample service stuff
    }
}
```