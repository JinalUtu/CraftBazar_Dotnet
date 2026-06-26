using CraftBazar_DTO.Common;
using CraftBazar_DTO.Users;

public interface IUserService
{
    Task<List<UserResponseDto>> GetAllUsersAsync();
    Task<UserResponseDto> GetUserByIdAsync(int id);
    Task<List<UserResponseDto>> GetUsersByRoleIdAsync(int roleId);
    Task<bool> UpdateUserDetailsAsync(UpdateUserRequestDto request);
    Task<bool> DeleteUserAsync(int id);
}
