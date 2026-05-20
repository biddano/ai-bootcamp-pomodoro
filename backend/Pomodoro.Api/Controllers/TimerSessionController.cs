namespace Pomodoro.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using Pomodoro.Api.Features.TimerSessions.GetTimerSessionHome;
using Pomodoro.Application.Features.TimerSessions.CreateTimerSession;

[ApiController]
[Route("api/[controller]")]
public class TimerSessionController : ControllerBase
{
    private readonly GetTimerSessionHomeHandler _getHomeHandler;
    private readonly CreateTimerSessionHandler _createHandler;
    private readonly CreateTimerSessionValidator _createValidator;

    public TimerSessionController(
        GetTimerSessionHomeHandler getHomeHandler,
        CreateTimerSessionHandler createHandler,
        CreateTimerSessionValidator createValidator)
    {
        _getHomeHandler = getHomeHandler;
        _createHandler = createHandler;
        _createValidator = createValidator;
    }

    [HttpGet("home")]
    public async Task<ActionResult<GetTimerSessionHomeResponse>> GetHome()
    {
        var request = new GetTimerSessionHomeRequest();
        var response = await _getHomeHandler.Handle(request);
        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<CreateTimerSessionResponse>> Create([FromBody] CreateTimerSessionRequest request)
    {
        var validationErrors = _createValidator.Validate(request);
        if (validationErrors.Count > 0)
        {
            return BadRequest(new { errors = validationErrors });
        }

        var response = await _createHandler.Handle(request);
        return CreatedAtAction(nameof(GetHome), response);
    }
}
