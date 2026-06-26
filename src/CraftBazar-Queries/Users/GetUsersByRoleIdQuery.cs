using MediatR;

namespace CraftBazar_Queries.Users;

public class GetUsersByRoleIdQuery : IRequest<List<UserResponseDto>>
{
    public int RoleId { get; set; }

}
