namespace Pomodoro.Application.Features.TimerSessions.CreateTimerSession;

using Pomodoro.Application.Services;

public class CreateTimerSessionHandler
{
    private readonly ITimerSessionApplicationService _applicationService;

    public CreateTimerSessionHandler(ITimerSessionApplicationService applicationService)
    {
        _applicationService = applicationService;
    }

    public async Task<CreateTimerSessionResponse> Handle(CreateTimerSessionRequest request)
    {
        var session = await _applicationService.CreateSessionAsync(request.Mode, request.KeyTask);

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
