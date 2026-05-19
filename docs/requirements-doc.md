# Pomodoro App V1 Product Requirements Document

## Overview

The Pomodoro app is a simple focus timer that helps users work in focused 25-minute sessions, take 5-minute breaks, and associate each focus session with one key task.

## Goal

Help users stay focused by providing a minimal timer workflow for alternating between focus sessions and breaks.

## V1 Scope

### Core Features

#### 1. Focus Timer

- User can start a 25-minute focus timer.
- Timer counts down from 25:00 to 00:00.
- User can stop/pause the timer.
- User can reset the timer back to 25:00.

#### 2. Break Timer

- User can switch to a 5-minute break timer.
- Timer counts down from 5:00 to 00:00.
- User can start, stop/pause, and reset the break timer.

#### 3. Timer Mode Switching

- User can switch between:
  - Focus mode: 25 minutes
  - Break mode: 5 minutes
- Switching modes updates the timer duration to the selected mode.
- If a timer is running, switching modes should reset the timer to the selected mode’s default duration.

#### 4. Timer Controls

The app should include the following controls:

- Start
- Stop/Pause
- Reset

#### 5. Key Task

- User can enter one key task for the current timer session.
- The task should be displayed while the timer is active.
- User can update or clear the task before starting a timer.

## Non-Goals for V1

The following are out of scope for the first version:

- User accounts
- Login/authentication
- Timer history
- Notifications
- Custom timer durations
- Task lists
- Calendar integration
- Analytics or reporting
- Multiple tasks per session
- Automatic Pomodoro cycles

## Basic User Flow

1. User opens the app.
2. User enters one key task.
3. User selects Focus mode or Break mode.
4. User starts the timer.
5. User can pause/stop or reset the timer.
6. User can switch between Focus and Break mode as needed.

## Success Criteria

V1 is complete when:

- User can run a 25-minute focus timer.
- User can run a 5-minute break timer.
- User can start, stop/pause, and reset the timer.
- User can switch between focus and break modes.
- User can enter and view one key task for the timer session.