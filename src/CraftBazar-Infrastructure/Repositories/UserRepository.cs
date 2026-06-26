
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Dapper;
using Microsoft.Extensions.Configuration;


public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;

    public UserRepository(ApplicationDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users.AsNoTracking()
            .Include(x => x.Role)
            .FirstOrDefaultAsync(x => x.Email == email);
    }
    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }
    private SqlConnection CreateConnection()
    {
        var connectionString = _configuration.GetConnectionString("DefaultConnection");
        return new SqlConnection(connectionString);
    }
    public async Task<List<User>> GetAllUsersAsync()
    {
        await using var connection = CreateConnection();
        await connection.OpenAsync();

        const string sql = @"
            SELECT
                u.Id,
                u.FirstName,
                u.LastName,
                u.Email,
                r.Name AS RoleName
            FROM Users u
            INNER JOIN Roles r ON u.RoleId = r.Id
            WHERE u.IsDeleted = 0";

        var users = await connection.QueryAsync<User>(sql);
        return users.AsList();
    }
    public async Task<User?> GetUserByIdAsync(int id)
    {
        await using var connection = CreateConnection();
        await connection.OpenAsync();

        // var sql = @"
        //     SELECT
        //         u.Id,
        //         u.FirstName,
        //         u.LastName,
        //         u.Email,
        //         r.Name AS RoleName
        //     FROM Users u
        //     INNER JOIN Roles r
        //         ON u.RoleId = r.Id
        //     WHERE u.Id = @UserId";
        var sql = @"
                SELECT
                    u.Id,
                    u.FirstName,
                    u.LastName,
                    u.Email,
                    u.RoleId,
                    r.Id,
                    r.Name
                FROM Users u
                INNER JOIN Roles r
                    ON u.RoleId = r.Id
                WHERE u.Id = @UserId";

        var user = (await connection.QueryAsync<User, Role, User>(
            sql,
            (user, role) =>
            {
                user.Role = role;
                return user;
            },
            new { UserId = id },
            splitOn: "Id"  // by default split by Id, if we do not add, it takes Id
        )).FirstOrDefault();
        return user;
    }
    public async Task<List<User?>> GetUsersByRoleIdAsync(int roleId)
    {
        await using var connection = CreateConnection();
        await connection.OpenAsync();

        var sql = @"
            SELECT
                u.Id,
                u.FirstName,
                u.LastName,
                u.Email,
                r.Name AS RoleName
            FROM Users u
            INNER JOIN Roles r
                ON u.RoleId = r.Id
            WHERE u.RoleId = @RoleId";

        var users = await connection.QueryAsync<User>(sql, new { RoleId = roleId });
        return users.AsList();
    }
    public async Task UpdateUserAsync(User user)
    {
        user.UpdatedDate = DateTime.UtcNow;
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteUserAsync(int id)
    {
        var user = await GetUserByIdAsync(id);
        if (user != null)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }
}

/**** Get using EF Core ****/
// public async Task<List<User>> GetUsersAsync()
// {
//     var users = await _context.Users.AsNoTracking().ToListAsync();
//     return users;
// }

// public async Task<User?> GetUserByIdAsync(int id)
// {
//     var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync((user) => user.Id == id);
//     return user;
// }

// public async Task<List<User>> GetUsersByRoleIdAsync(int roleId)
// {
//     var users = await _context.Users.AsNoTracking().Where((user) => user.RoleId == roleId).ToListAsync();
//     return users;
// }

