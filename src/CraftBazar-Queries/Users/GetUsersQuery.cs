using MediatR;

namespace CraftBazar_Queries.Users;

public class GetUsersQuery : IRequest<List<UserResponseDto>>
{
}
