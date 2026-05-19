namespace Pomodoro.Api.Features.TimerSessions.GetTimerSessionHome;

using Pomodoro.Api.Models;
using Pomodoro.Api.Services;

public class GetTimerSessionHomeHandler
{
    private readonly ITimerSessionService _timerSessionService;

    public GetTimerSessionHomeHandler(ITimerSessionService timerSessionService)
    {
        _timerSessionService = timerSessionService;
    }

    public async Task<GetTimerSessionHomeResponse> Handle(GetTimerSessionHomeRequest request)
    {
        var session = await _timerSessionService.GetOrCreateHomeSessionAsync();

        return new GetTimerSessionHomeResponse
        {
            SessionId = session.Id,
            Mode = session.Mode.ToString(),
            Status = session.Status.ToString(),
            KeyTask = session.KeyTask,
            DurationSeconds = session.GetDurationSeconds(),
            ElapsedSeconds = session.ElapsedSeconds,
            RemainingSeconds = session.GetRemainingSeconds()
        };
    }
}
