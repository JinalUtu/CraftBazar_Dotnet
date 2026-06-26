using AutoMapper;
using CraftBazar.Commands.Authentication.Login;
using CraftBazar.Commands.Authentication.Register;
using CraftBazar_DTO.Common;
using MediatR;
public class AuthService : IAuthService
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    public AuthService(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    public async Task<bool> RegisterAsync(RegisterRequestDto dto)
    {
        // Automatically maps the DTO into the RegisterCommand object
        var command = _mapper.Map<RegisterCommand>(dto);
        return await _mediator.Send(command);
    }

    public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto dto)
    {
        // Automatically maps the DTO into the LoginCommand object
        var command = _mapper.Map<LoginCommand>(dto);
        return await _mediator.Send(command);
    }
}
