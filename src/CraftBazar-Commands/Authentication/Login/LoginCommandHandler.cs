using CraftBazar.Commands.Authentication.Login;
using MediatR;

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponseDto?>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordService _passwordService;
    private readonly IJwtService _jwtService;
    public LoginCommandHandler(IUserRepository userRepository, IPasswordService passwordService, IJwtService jwtService)
    {
        _userRepository = userRepository;
        _passwordService = passwordService;
        _jwtService = jwtService;
    }

    public async Task<LoginResponseDto?> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        // 1. Fetch user from infrastructure
        var user = await _userRepository.GetByEmailAsync(request.Email);

        // 2. Validate user existence and password validity
        if (user == null || !_passwordService.VerifyPassword(request.Password, user.PasswordHash))
        {
            throw new Exception("Invalid email or password.");
        }

        // 3. Generate token if valid
        var token = _jwtService.GenerateToken(user);

        // 4. Return response
        return new LoginResponseDto
        {
            token = token
        };
    }
}
