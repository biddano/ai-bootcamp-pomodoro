namespace Pomodoro.Api.Features.TimerSessions.CreateTimerSession;

using Pomodoro.Api.Models;
using Pomodoro.Api.Services;

public class CreateTimerSessionHandler
{
    private readonly ITimerSessionService _timerSessionService;

    public CreateTimerSessionHandler(ITimerSessionService timerSessionService)
    {
        _timerSessionService = timerSessionService;
    }

    public async Task<CreateTimerSessionResponse> Handle(CreateTimerSessionRequest request)
    {
        var session = await _timerSessionService.CreateSessionAsync(request.Mode, request.KeyTask);

        return new CreateTimerSessionResponse
        {
            SessionId = session.Id,
            Mode = session.Mode.ToString(),
            Status = session.Status.ToString(),
            KeyTask = session.KeyTask,
            DurationSeconds = session.GetDurationSeconds(),
            ElapsedSeconds = session.ElapsedSeconds,
            RemainingSeconds = session.GetRemainingSeconds(),
            CreatedAt = session.CreatedAt
        };
    }
}
