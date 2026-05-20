namespace Pomodoro.Application.Features.TimerSessions.CreateTimerSession;

using Pomodoro.Domain.Entities;

public class CreateTimerSessionRequest
{
    public TimerMode Mode { get; set; }
    public string? KeyTask { get; set; }
}
