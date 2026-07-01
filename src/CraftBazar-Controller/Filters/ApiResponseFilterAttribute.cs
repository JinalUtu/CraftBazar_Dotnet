using CraftBazar_DTO.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CraftBazar_Controller.Filters;

public class ApiResponseFilterAttribute : ResultFilterAttribute
{
    public override void OnResultExecuting(ResultExecutingContext context)
    {
        // Skip if already wrapped
        if (context.Result is ObjectResult objectResult &&
            objectResult.Value is ApiResponse<object>)
        {
            return;
        }

        // Handle ObjectResult (Ok, Created, BadRequest, NotFound(value), etc.)
        if (context.Result is ObjectResult objectResponse)
        {
            int statusCode = objectResponse.StatusCode ?? StatusCodes.Status200OK;

            var response = new ApiResponse<object>
            {
                Success = statusCode >= 200 && statusCode < 300,
                Message = GetDefaultMessage(statusCode),
                Data = statusCode >= 200 && statusCode < 300
                    ? objectResponse.Value
                    : null,
                Errors = statusCode >= 400
                    ? new List<string> { objectResponse.Value?.ToString() ?? "Request failed." }
                    : null,

            };

            context.Result = new ObjectResult(response)
            {
                StatusCode = statusCode
            };
        }

        // Handle responses like NoContent(), Unauthorized(), Forbid()
        else if (context.Result is StatusCodeResult statusResult)
        {
            int statusCode = statusResult.StatusCode;

            context.Result = new ObjectResult(new ApiResponse<object>
            {
                Success = statusCode >= 200 && statusCode < 300,
                Message = GetDefaultMessage(statusCode),
                Errors = statusCode >= 400
                    ? new List<string> { GetDefaultMessage(statusCode) }
                    : null
            })
            {
                StatusCode = statusCode
            };
        }
    }

    private static string GetDefaultMessage(int statusCode)
    {
        return statusCode switch
        {
            StatusCodes.Status200OK => "Request completed successfully.",
            StatusCodes.Status201Created => "Resource created successfully.",
            StatusCodes.Status204NoContent => "Request completed successfully.",
            StatusCodes.Status400BadRequest => "Bad request.",
            StatusCodes.Status401Unauthorized => "Unauthorized.",
            StatusCodes.Status403Forbidden => "Forbidden.",
            StatusCodes.Status404NotFound => "Resource not found.",
            StatusCodes.Status500InternalServerError => "Internal server error.",
            _ => "Request processed."
        };
    }
}