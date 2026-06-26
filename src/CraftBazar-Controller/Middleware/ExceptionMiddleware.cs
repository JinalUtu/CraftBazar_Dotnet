using System.Net;
using System.Text.Json;
using CraftBazar_DTO.Common;
using FluentValidation;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(
        RequestDelegate next,
        ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(
        HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            context.Response.ContentType = "application/json";

            if (ex is ValidationException validationEx)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                var messages = validationEx.Errors?
                    .Select(e => e.ErrorMessage)
                    .Where(m => !string.IsNullOrWhiteSpace(m))
                    .ToList() ?? new List<string>();

                var message = messages.Count == 1 ? messages.First() : string.Join("; ", messages);

                var response = ApiResponse<object>.FailureResponse(message, messages);

                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                return;
            }

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var genericResponse = ApiResponse<object>.FailureResponse(
                ex.Message,
                new List<string> { ex.Message });

            await context.Response.WriteAsync(JsonSerializer.Serialize(genericResponse));
        }
    }
}
