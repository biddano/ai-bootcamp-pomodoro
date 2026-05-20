namespace Pomodoro.Api.Features.TimerSessions.GetTimerSessionHome;

using Pomodoro.Application.Services;

public class GetTimerSessionHomeHandler
{
    private readonly ITimerSessionApplicationService _applicationService;

    public GetTimerSessionHomeHandler(ITimerSessionApplicationService applicationService)
    {
        _applicationService = applicationService;
    }

    public async Task<GetTimerSessionHomeResponse> Handle(GetTimerSessionHomeRequest request)
    {
        var session = await _applicationService.GetOrCreateHomeSessionAsync();

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
