namespace Pomodoro.Api.Services;

using Pomodoro.Api.Models;

public interface ITimerSessionService
{
    Task<TimerSession> GetOrCreateHomeSessionAsync();
    Task<TimerSession?> GetSessionByIdAsync(string id);
    Task<TimerSession> CreateSessionAsync(TimerMode mode, string? keyTask);
}
