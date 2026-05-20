namespace Pomodoro.Api.Features.TimerSessions.CreateTimerSession;

using Pomodoro.Api.Models;

public class CreateTimerSessionRequest
{
    public TimerMode Mode { get; set; }
    public string? KeyTask { get; set; }
}
