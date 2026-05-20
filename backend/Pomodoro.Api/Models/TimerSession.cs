namespace Pomodoro.Api.Models;

public enum TimerMode
{
    Focus,
    Break
}

public enum TimerStatus
{
    Idle,
    Running,
    Paused,
    Completed
}

public class TimerSession
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public TimerMode Mode { get; set; } = TimerMode.Focus;
    public TimerStatus Status { get; set; } = TimerStatus.Idle;
    public string? KeyTask { get; set; }
    public int ElapsedSeconds { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedAt { get; set; }

    public int GetDurationSeconds() =>
        Mode switch
        {
            TimerMode.Focus => 25 * 60,
            TimerMode.Break => 5 * 60,
            _ => 25 * 60
        };

    public int GetRemainingSeconds() => GetDurationSeconds() - ElapsedSeconds;

    public bool IsActive() => Status == TimerStatus.Running || Status == TimerStatus.Paused;
}
