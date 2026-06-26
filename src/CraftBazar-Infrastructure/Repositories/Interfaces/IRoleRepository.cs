public interface IRoleRepository
{
    Task<bool> RoleExistsAsync(int roleId);
}