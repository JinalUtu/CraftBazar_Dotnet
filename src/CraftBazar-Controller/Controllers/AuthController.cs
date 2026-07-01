using CraftBazar_Controller.Jobs;
using Hangfire;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IBackgroundJobClient _backgroundJobClient;
    // private readonly IRecurringJobManager _recurringJob;

    public AuthController(
        IAuthService authService,
        IBackgroundJobClient backgroundJobClient,
        IRecurringJobManager recurringJob)
    {
        _authService = authService;
        _backgroundJobClient = backgroundJobClient;
        // _recurringJob = recurringJob;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto dto)
    {
        var result = await _authService.RegisterAsync(dto);

        if (result)
        {
            var RegistrationTime = DateTime.UtcNow;
            /* Immediate job */
            _backgroundJobClient.Enqueue<RegistrationEmailJob>(job => job.SendRegistrationEmail(dto.Email));
            /* Delayed job */
            // _backgroundJobClient.Schedule<RegistrationEmailJob>(job => job.SendRegistrationEmail(dto.Email), TimeSpan.FromMinutes(1));
            /* Recurring job */
            // _recurringJob.AddOrUpdate<RegistrationEmailJob>("test-recurring-job", job => job.SendRegistrationEmail(dto.Email, RegistrationTime), Cron.Minutely());
            /* Continuation Jobs - Run Job A succeeds. */
            // var jobId = BackgroundJob.Enqueue<RegistrationEmailJob>(job => job.SendRegistrationEmail("user@gmail.com"));
            // BackgroundJob.ContinueJobWith<RegistrationEmailJob>(jobId, job => job.SendRegistrationEmail("admin@gmail.com"));
            /* Test Failure */
            // _backgroundJobClient.Enqueue<RegistrationEmailJob>(job => job.TestFailure());

            return Ok();
        }

        return BadRequest("Registration failed.");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
    {
        var result = await _authService.LoginAsync(dto);

        if (result == null)
        {
            return BadRequest("Login failed.");
        }

        return Ok(result);
    }
}
