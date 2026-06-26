public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task AddAsync(User user);
    Task<List<User>> GetAllUsersAsync();
    Task<User?> GetUserByIdAsync(int id);
    Task<List<User>> GetUsersByRoleIdAsync(int roleId);
    Task UpdateUserAsync(User user);
    Task DeleteUserAsync(int id);
}