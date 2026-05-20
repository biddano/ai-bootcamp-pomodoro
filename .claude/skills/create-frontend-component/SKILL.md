---
name: create-frontend-component
description: Generate a new React TypeScript presentation or data component with matching interface types and static/mock initialization states.
---

Creates a new UI view or presentation component for the PomoTimer application.

**Interactive prompts:**
- Asks for the component or domain view name (e.g., PomodoroSession, TaskList, TimerDisplay)
- Asks what properties/fields the view or underlying data object should have (comma-separated list)

**Process:**
1. Validates the component name matches standard PascalCase React conventions.
2. Creates a new `.tsx` file in the `./frontend/src/components/` directory.
3. Generates an explicit TypeScript interface defining the data shapes and component Props.
4. Uses standard functional definitions (`export function ComponentName()`) instead of variable arrow expressions.
5. Injects primitive styling via Tailwind CSS classes inline.
6. Includes standard JSDoc block documentation headers for the component and its data properties.

**Output:**
- Creates a modern, type-safe React component matching workspace UI boundaries.
- Uses strict optional/nullable TypeScript declarations (`?` or `| null`).
- Provides a modular foundation complete with layout styling.

**Example output:**
```tsx
import React from 'react';

/**
 * Properties representing a Pomodoro work session data object.
 */
export interface PomodoroSessionProps {
  id: string;
  name: string;
  durationMinutes: number;
  onSelect?: (id: string) => void;
}

/**
 * Component representing a Pomodoro work session row or card layout.
 */
export function PomodoroSession({ id, name, durationMinutes, onSelect }: PomodoroSessionProps) {
  return (
    <div className="p-4 mb-3 border rounded-xl bg-card text-card-foreground shadow-sm hover:shadow-md transition-shadow">
      <div className="flex items-center justify-between">
        <div>
          <h3 className="text-lg font-semibold tracking-tight text-foreground">{name}</h3>
          <p className="text-sm text-muted-foreground">{durationMinutes} minutes</p>
        </div>
        {onSelect && (
          <button
            onClick={() => onSelect(id)}
            className="px-3 py-1.5 text-sm font-medium text-primary-foreground bg-primary rounded-lg hover:bg-primary/90 transition-colors"
          >
            Select Session
          </button>
        )}
      </div>
    </div>
  );
}