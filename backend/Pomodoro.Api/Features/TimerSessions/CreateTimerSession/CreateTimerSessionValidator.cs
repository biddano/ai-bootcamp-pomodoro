namespace Pomodoro.Api.Features.TimerSessions.CreateTimerSession;

using Pomodoro.Api.Models;

public class CreateTimerSessionValidator
{
    public List<string> Validate(CreateTimerSessionRequest request)
    {
        var errors = new List<string>();

        if (!Enum.IsDefined(typeof(TimerMode), request.Mode))
        {
            errors.Add("Mode must be a valid TimerMode value (Focus or Break).");
        }

        if (request.KeyTask != null && request.KeyTask.Length > 200)
        {
            errors.Add("KeyTask must not exceed 200 characters.");
        }

        return errors;
    }
}
