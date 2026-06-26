using Dapper;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace CraftBazar_Queries.Users;

public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, List<UserResponseDto>>
{
    private readonly IConfiguration _configuration;

    public GetUsersQueryHandler(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<List<UserResponseDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var connectionString = _configuration.GetConnectionString("DefaultConnection");

        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync(cancellationToken);

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

        var users = await connection.QueryAsync<UserResponseDto>(sql);
        return users.AsList();
    }
}
