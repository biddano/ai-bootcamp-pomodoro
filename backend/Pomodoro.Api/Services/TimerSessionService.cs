namespace Pomodoro.Api.Services;

using Pomodoro.Api.Models;

public class TimerSessionService : ITimerSessionService
{
    private readonly Dictionary<string, TimerSession> _sessions = new();
    private string? _currentSessionId;

    public async Task<TimerSession> GetOrCreateHomeSessionAsync()
    {
        if (_currentSessionId != null && _sessions.TryGetValue(_currentSessionId, out var existingSession))
        {
            return await Task.FromResult(existingSession);
        }

        var newSession = new TimerSession();
        _sessions[newSession.Id] = newSession;
        _currentSessionId = newSession.Id;

        return await Task.FromResult(newSession);
    }

    public async Task<TimerSession?> GetSessionByIdAsync(string id)
    {
        _sessions.TryGetValue(id, out var session);
        return await Task.FromResult(session);
    }

    public async Task<TimerSession> CreateSessionAsync(TimerMode mode, string? keyTask)
    {
        var session = new TimerSession
        {
            Mode = mode,
            KeyTask = keyTask,
            Status = TimerStatus.Idle,
            ElapsedSeconds = 0,
            CreatedAt = DateTime.UtcNow
        };

        _sessions[session.Id] = session;
        return await Task.FromResult(session);
    }
}
