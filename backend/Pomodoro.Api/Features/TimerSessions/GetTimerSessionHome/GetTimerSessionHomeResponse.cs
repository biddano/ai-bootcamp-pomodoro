namespace Pomodoro.Api.Features.TimerSessions.GetTimerSessionHome;

public class GetTimerSessionHomeResponse
{
    public string SessionId { get; set; } = string.Empty;
    public string Mode { get; set; } = "Focus";
    public string Status { get; set; } = "Idle";
    public string? KeyTask { get; set; }
    public int DurationSeconds { get; set; }
    public int ElapsedSeconds { get; set; }
    public int RemainingSeconds { get; set; }
}
