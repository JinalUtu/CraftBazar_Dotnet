public interface IAuthService
{
    Task<bool> RegisterAsync(RegisterRequestDto dto);
    Task<LoginResponseDto?> LoginAsync(LoginRequestDto dto);
}
