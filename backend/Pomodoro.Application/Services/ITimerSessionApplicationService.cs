namespace Pomodoro.Application.Services;

using Pomodoro.Domain.Entities;

public interface ITimerSessionApplicationService
{
    Task<TimerSession> CreateSessionAsync(TimerMode mode, string? keyTask);
    Task<TimerSession?> GetSessionByIdAsync(string id);
    Task<TimerSession> GetOrCreateHomeSessionAsync();
}
