public interface IJSONPlaceHolderService
{
    Task<string> GetUsersAsync();
    Task<JsonUserRequestDto?> CreateUserAsync(JsonUserRequestDto user);
    Task<JsonUserRequestDto?> UpdateUserAsync(int id, JsonUserRequestDto user);
    Task<JsonUserRequestDto?> PatchUserAsync(int id, object update);
    Task<bool> DeleteUserAsync(int id);
}