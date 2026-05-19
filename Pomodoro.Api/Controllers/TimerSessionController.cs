namespace Pomodoro.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using Pomodoro.Api.Features.TimerSessions.GetTimerSessionHome;

[ApiController]
[Route("api/[controller]")]
public class TimerSessionController : ControllerBase
{
    private readonly GetTimerSessionHomeHandler _getHomeHandler;

    public TimerSessionController(GetTimerSessionHomeHandler getHomeHandler)
    {
        _getHomeHandler = getHomeHandler;
    }

    [HttpGet("home")]
    public async Task<ActionResult<GetTimerSessionHomeResponse>> GetHome()
    {
        var request = new GetTimerSessionHomeRequest();
        var response = await _getHomeHandler.Handle(request);
        return Ok(response);
    }
}
