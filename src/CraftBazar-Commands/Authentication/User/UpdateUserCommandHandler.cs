using MediatR;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, bool>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordService _passwordService;

    public UpdateUserCommandHandler(
        IUserRepository userRepository,
        IPasswordService passwordService)
    {
        _userRepository = userRepository;
        _passwordService = passwordService;
    }

    public async Task<bool> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByIdAsync(request.Id);
        if (user == null)
        {
            return false;
        }

        if (!string.IsNullOrWhiteSpace(request.FirstName))
            user.FirstName = request.FirstName;

        if (!string.IsNullOrWhiteSpace(request.LastName))
            user.LastName = request.LastName;

        if (!string.IsNullOrWhiteSpace(request.Email))
            user.Email = request.Email;

        if (!string.IsNullOrWhiteSpace(request.Password))
            user.PasswordHash = _passwordService.HashPassword(request.Password);

        if (request.RoleId.HasValue)
            user.RoleId = request.RoleId.Value;

        await _userRepository.UpdateUserAsync(user);

        return true;
    }
}
