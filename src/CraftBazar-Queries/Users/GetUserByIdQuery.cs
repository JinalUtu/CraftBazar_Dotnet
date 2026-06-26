using MediatR;

namespace CraftBazar_Queries.Users;

public class GetUserByIdQuery : IRequest<UserResponseDto>
{
    public int UserId { get; set; }

}
