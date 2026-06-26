using CraftBazar_DTO.Common;
using CraftBazar_DTO.Users;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/users")]
[Produces("application/json")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(
        IUserService userService)
    {
        _userService = userService;
    }

    // [Authorize]
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<UserResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetById(int id)
    {
        var result =
            await _userService
                .GetUserByIdAsync(id);

        if (result == null)
        {
            return NotFound(ApiResponse<UserResponseDto>.FailureResponse(
                "User not found.",
                new List<string> { "No user exists with the provided id." }));
        }

        return Ok(ApiResponse<UserResponseDto>.SuccessResponse(
            result,
            "User retrieved successfully."));
    }

    // [Authorize]
    [HttpPut("{id}")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateUserRequestDto request)
    {
        request.Id = id;
        var result = await _userService.UpdateUserDetailsAsync(request);
        if (!result)
        {
            return NotFound(ApiResponse<object>.FailureResponse(
                "User not found.",
                new List<string> { "User not found." }));
        }

        return Ok(ApiResponse<object?>.SuccessResponse(
            null,
            "Data Updated Successfully"));
    }

    // [Authorize(Roles = "Admin")]
    // [Authorize]
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<List<UserResponseDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll()
    {
        var result =
            await _userService
                .GetAllUsersAsync();

        return Ok(ApiResponse<List<UserResponseDto>>.SuccessResponse(
            result,
            "User retrieved successfully."
        ));
    }

    // [Authorize(Roles = "Admin")]
    [HttpGet("role/{roleId}")]
    [ProducesResponseType(typeof(ApiResponse<List<UserResponseDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetByRoleId(int roleId)
    {
        var result = await _userService.GetUsersByRoleIdAsync(roleId);
        return Ok(ApiResponse<List<UserResponseDto>>.SuccessResponse(
            result,
            "Users retrieved successfully."));
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _userService.DeleteUserAsync(id);
        if (!result)
        {
            return NotFound(ApiResponse<object>.FailureResponse(
                "User not found.",
                new List<string> { "User not found." }));
        }

        return Ok(ApiResponse<object?>.SuccessResponse(
            null,
            "User deleted successfully."));
    }
}
