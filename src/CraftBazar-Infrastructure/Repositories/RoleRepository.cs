
using Microsoft.EntityFrameworkCore;

public class RoleRepository : IRoleRepository
{
    private readonly ApplicationDbContext _context;

    public RoleRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> RoleExistsAsync(int roleId)
    {
        return await _context.Roles.AnyAsync(x => x.Id == roleId);
    }
}