using CraftBazar_Queries.Users;
using MediatR;

public class GetUserByIdQueryHandler
    : IRequestHandler<GetUserByIdQuery, UserResponseDto?>
{
    private readonly IUserRepository _userRepository;

    public GetUserByIdQueryHandler(
        IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserResponseDto?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByIdAsync(request.UserId);
        if (user == null) return null;

        return new UserResponseDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            RoleName = user.Role?.Name ?? string.Empty
        };
    }
}