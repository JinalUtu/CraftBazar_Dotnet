using CraftBazar.Commands.Authentication.Register;
using CraftBazar_DTO.Common;
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
    public async Task<ActionResult<ApiResponse<object?>>> Register([FromBody] RegisterRequestDto dto)
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

            return Ok(ApiResponse<object>.SuccessResponse(
                null,
                "Registration successful."));
        }

        return BadRequest(ApiResponse<object>.FailureResponse(
            "Registration failed.",
            new List<string> { "Registration could not be completed." }));
    }

    [HttpPost("login")]
    public async Task<ActionResult<ApiResponse<LoginResponseDto>>> Login([FromBody] LoginRequestDto dto)
    {
        var result = await _authService.LoginAsync(dto);

        if (result == null)
        {
            return BadRequest(ApiResponse<LoginResponseDto>.FailureResponse(
                "Login failed.",
                new List<string> { "Invalid email or password." }));
        }

        return Ok(ApiResponse<LoginResponseDto>.SuccessResponse(
            result,
            "Login successful."));
    }
}
