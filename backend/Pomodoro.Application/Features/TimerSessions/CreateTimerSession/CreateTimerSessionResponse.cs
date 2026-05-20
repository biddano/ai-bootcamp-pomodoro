namespace Pomodoro.Application.Features.TimerSessions.CreateTimerSession;

public class CreateTimerSessionResponse
{
    public string SessionId { get; set; } = string.Empty;
    public string Mode { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string? KeyTask { get; set; }
    public int DurationSeconds { get; set; }
    public int ElapsedSeconds { get; set; }
    public int RemainingSeconds { get; set; }
    public DateTime CreatedAt { get; set; }
}
