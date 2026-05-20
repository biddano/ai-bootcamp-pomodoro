# Pomodoro App V1 UX Spec

## Overview

The Pomodoro app is a minimal focus timer that helps users run 25-minute focus sessions and 5-minute break sessions. The design should be inspired by the provided Pomofocus screenshot, with a centered timer card, clear mode switching, and a simple task area.

The final UI should use a cool dark mode theme with a high-contrast accent color.

## Design Goals

* Make the timer the main visual focus.
* Keep the interface calm, modern, and distraction-free.
* Support only the essential V1 actions.
* Use strong contrast for readability and key actions.
* Avoid unnecessary navigation, settings, reports, accounts, or menus.

## Color Palette

| Purpose              |         Color | Hex       |
| -------------------- | ------------: | --------- |
| App background       |     Deep navy | `#0F172A` |
| Primary surface/card |         Slate | `#1E293B` |
| Secondary surface    | Lighter slate | `#334155` |
| Primary text         |    Near white | `#F8FAFC` |
| Secondary text       |     Cool gray | `#CBD5E1` |
| Muted text           |    Slate gray | `#94A3B8` |
| Accent color         |          Cyan | `#22D3EE` |
| Accent hover         |   Bright cyan | `#67E8F9` |
| Accent active/dark   |     Deep cyan | `#0891B2` |
| Border/divider       |  Slate border | `#475569` |
| Error/clear action   |      Soft red | `#F87171` |

## Typography

Use a clean sans-serif font.

Recommended font stack:

`Inter, system-ui, -apple-system, BlinkMacSystemFont, "Segoe UI", sans-serif`

Text sizing:

| Element           |  Size | Weight |
| ----------------- | ----: | -----: |
| App title         |  28px |    700 |
| Timer mode labels |  18px |    600 |
| Timer display     | 112px |    700 |
| Primary button    |  24px |    700 |
| Section heading   |  24px |    700 |
| Task input        |  18px |    400 |

## Page Layout

The app should use one primary screen.

Structure:

1. Header
2. Centered timer card
3. Session/task message
4. Key task section

The page should have a max content width of approximately `900px` and be centered horizontally.

## Header

The header should include:

* App name: `Pomodoro`
* Optional simple checkmark or timer icon

The header should not include:

* Reports
* Settings
* Sign in
* Menu button

V1 should stay focused on the timer experience.

## Timer Card

The timer card is the main component on the page.

Style:

* Background: `#1E293B`
* Border: `1px solid #334155`
* Border radius: `20px`
* Padding: `40px`
* Width: full available width up to approximately `720px`
* Centered on the page

## Timer Mode Selector

The timer card should include two mode options:

* `Focus`
* `Break`

Focus mode:

* Duration: 25 minutes
* Default timer: `25:00`

Break mode:

* Duration: 5 minutes
* Default timer: `05:00`

Active mode styling:

* Background: `#0891B2`
* Text: `#F8FAFC`
* Border radius: `10px`

Inactive mode styling:

* Text: `#CBD5E1`
* Background: transparent

Hover state:

* Background: `#334155`
* Text: `#F8FAFC`

## Timer Display

The timer display should be the largest and most prominent element.

Default focus timer:

`25:00`

Default break timer:

`05:00`

Style:

* Text color: `#F8FAFC`
* Font size: `112px`
* Font weight: `700`
* Letter spacing: `-2px`
* Center aligned

## Timer Controls

The app should include three core control actions:

* Start
* Pause
* Reset

### Start Button

Default label:

`START`

Style:

* Background: `#22D3EE`
* Hover background: `#67E8F9`
* Text color: `#0F172A`
* Border radius: `12px`
* Font size: `24px`
* Font weight: `700`
* Width: approximately `320px`
* Height: approximately `72px`

### Pause Button

When the timer is running, the Start button changes to:

`PAUSE`

Use the same styling as the Start button.

### Reset Button

The Reset button should be secondary and less visually prominent.

Style:

* Background: transparent
* Border: `1px solid #475569`
* Text color: `#CBD5E1`
* Hover background: `#334155`
* Hover text: `#F8FAFC`

## Session Message

Below the timer card, show a short session message.

Default focus mode message:

`Time to focus.`

Default break mode message:

`Take a short break.`

Style:

* Text color: `#CBD5E1`
* Font size: `20px`
* Center aligned

## Key Task Section

The key task section should appear below the timer area.

Purpose:

Allow the user to enter one key task for the current timer session.

Section label:

`Key Task`

Input placeholder:

`What are you focusing on?`

Style:

* Section heading color: `#F8FAFC`
* Input background: `#1E293B`
* Input border: `1px solid #475569`
* Input text: `#F8FAFC`
* Placeholder text: `#94A3B8`
* Focus border: `#22D3EE`
* Border radius: `12px`

## Key Task Behavior

User can:

* Enter one task
* View the task while the timer is active
* Edit the task before starting the timer
* Clear the task before starting the timer

When the timer is running:

* The task should remain visible.
* Editing can be disabled for V1 to keep behavior simple.
* The clear button should be hidden or disabled.

## Interaction Rules

### Start Timer

When the user clicks `START`:

* Timer begins counting down.
* Button changes to `PAUSE`.
* Current mode remains selected.
* Key task remains visible.

### Pause Timer

When the user clicks `PAUSE`:

* Timer stops counting down.
* Button changes back to `START`.
* Timer keeps the current remaining time.

### Reset Timer

When the user clicks `RESET`:

* Timer stops.
* Timer returns to the selected mode’s default duration.
* Focus resets to `25:00`.
* Break resets to `05:00`.

### Switch Modes

When the user switches between Focus and Break:

* Timer stops if currently running.
* Timer resets to the selected mode’s default duration.
* Active mode styling updates.
* Session message updates.

## V1 Screen Content

Initial screen state:

* Header: `Pomodoro`
* Active mode: `Focus`
* Timer: `25:00`
* Primary button: `START`
* Secondary button: `RESET`
* Message: `Time to focus.`
* Key Task input: `What are you focusing on?`

## Out of Scope for V1

* User accounts
* Sign in
* Reports
* Settings
* Long break mode
* Multiple tasks
* Task history
* Notifications
* Sound effects
* Custom timer durations
* Automatic Pomodoro cycles
* Analytics
