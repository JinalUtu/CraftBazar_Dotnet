using CraftBazar_DTO.Common;
using MediatR;

namespace CraftBazar.Commands.Authentication.Register;

public class RegisterCommand : IRequest<bool>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public int RoleId { get; set; }
}
