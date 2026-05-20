using Pomodoro.Api.Features.TimerSessions.GetTimerSessionHome;
using Pomodoro.Application.Features.TimerSessions.CreateTimerSession;
using Pomodoro.Application.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<ITimerSessionApplicationService, TimerSessionApplicationService>();
builder.Services.AddScoped<GetTimerSessionHomeHandler>();
builder.Services.AddScoped<CreateTimerSessionHandler>();
builder.Services.AddScoped<CreateTimerSessionValidator>();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
