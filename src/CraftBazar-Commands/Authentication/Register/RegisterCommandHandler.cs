using CraftBazar.Commands.Authentication.Register;
using CraftBazar_DTO.Common;
using MediatR;

public class RegisterCommandHandler
    : IRequestHandler<RegisterCommand, bool>
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IPasswordService _passwordService;

    public RegisterCommandHandler(
        IUserRepository userRepository,
        IRoleRepository roleRepository,
        IPasswordService passwordService)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _passwordService = passwordService;
    }

    public async Task<bool> Handle(
        RegisterCommand request,
        CancellationToken cancellationToken)
    {
        // Check Email Exists
        var existingUser = await _userRepository.GetByEmailAsync(request.Email);
        if (existingUser != null)
        {
            throw new Exception(
                "Email already registered.");
        }

        // Check Role Exists
        var roleExists = await _roleRepository.RoleExistsAsync(request.RoleId);
        if (!roleExists)
        {
            throw new Exception(
                "Invalid role selected.");
        }

        // Prevent Admin Registration
        if (request.RoleId == 1)
        {
            throw new Exception(
                "Admin registration is not allowed.");
        }

        // Create User
        var user = new User
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            PasswordHash =
                _passwordService.HashPassword(
                    request.Password),
            RoleId = request.RoleId,
            CreatedDate = DateTime.UtcNow,
            IsDeleted = false
        };

        await _userRepository.AddAsync(user);

        return true;
    }
}
