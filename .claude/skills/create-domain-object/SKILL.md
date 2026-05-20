---
name: create-domain-object
description: Generate a new domain object class with properties and static create method.
---

Creates a new domain object (entity/model) for the PomoTimer application.

**Interactive prompts:**
- Asks for domain object name (e.g., PomodoroSession, Task, Timer)
- Asks what properties the object should have (comma-separated list)

**Process:**
1. Validates the domain object name is a valid C# identifier
2. Creates a new .cs file in `./src/backend/Domain/` directory
3. Generates a public class with readonly properties for each specified property
3. Has a private constructor.
4. Generate static method to create the domain object and populate the properties.
5. Adds XML documentation comments to class and properties

**Output:**
- Creates a well-structured domain object file with proper C# conventions
- Includes nullable reference type annotations
- Provides a foundation for further development

**Example output:**
```csharp
namespace PomoTimer.Domain;

/// <summary>
/// Represents a Pomodoro work session
/// </summary>
public class PomodoroSession
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public int DurationMinutes { get; private set; }
    
    public PomodoroSession(string name, int durationMinutes)
    {
        Name = name;
        DurationMinutes = durationMinutes;
    }

    public static PomodoroSession Create(Guid id, string name, int durationMinutes)
    {
        var newSession = new() {
            Id = id,
            Name = name,
            DurationMinutes = durationMinutes
        };

        return newSession;
    }
}
```
