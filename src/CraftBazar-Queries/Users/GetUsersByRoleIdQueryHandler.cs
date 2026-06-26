using CraftBazar_Queries.Users;
using Dapper;
using MediatR;

public class GetUsersByRoleIdQueryHandler : IRequestHandler<GetUsersByRoleIdQuery, List<UserResponseDto>>
{
    private readonly IUserRepository _userRepository;

    public GetUsersByRoleIdQueryHandler(
        IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<List<UserResponseDto>> Handle(GetUsersByRoleIdQuery request, CancellationToken cancellationToken)
    {
        var users = await _userRepository.GetUsersByRoleIdAsync(request.RoleId);
        if (users == null || users.Count == 0) return new List<UserResponseDto>();

        return users.Select(user => new UserResponseDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            RoleName = user.Role?.Name ?? string.Empty
        }).ToList();
    }
}