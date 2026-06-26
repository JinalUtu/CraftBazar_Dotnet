using CraftBazar_DTO.Common;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/demo/users")]
[Tags("Third Party API Demo using HttpClient")]
public class DemoJSONController : ControllerBase
{
    private readonly IJSONPlaceHolderService _jsonService;
    public DemoJSONController(IJSONPlaceHolderService jsonService)
    {
        _jsonService = jsonService;
    }

    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        var result = await _jsonService.GetUsersAsync();
        return Ok(ApiResponse<string>.SuccessResponse(
            result,
            "Users retrieved successfully."));
    }

    [HttpPost]
    public async Task<IActionResult> Create(JsonUserRequestDto user)
    {
        var result = await _jsonService.CreateUserAsync(user);
        return Ok(ApiResponse<JsonUserRequestDto?>.SuccessResponse(
            result,
            "User created successfully."));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, JsonUserRequestDto user)
    {
        var result = await _jsonService.UpdateUserAsync(id, user);
        return Ok(ApiResponse<JsonUserRequestDto?>.SuccessResponse(
            result,
            "User updated successfully."));
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateUser(int id, JsonUserRequestDto user)
    {
        var result = await _jsonService.PatchUserAsync(id, user);
        return Ok(ApiResponse<JsonUserRequestDto?>.SuccessResponse(
            result,
            "User updated successfully."));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _jsonService.DeleteUserAsync(id);

        if (!deleted)
            return BadRequest(ApiResponse<object>.FailureResponse(
                "User delete failed.",
                new List<string> { "Unable to delete user." }));

        return Ok(ApiResponse<object>.SuccessResponse(
            null,
            "User deleted successfully."));
    }
}
