using AutoMapper;
using MediatR;
using CraftBazar_Queries.Users;
using CraftBazar_DTO.Users;

public class UserService : IUserService
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    public UserService(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    public async Task<List<UserResponseDto>> GetAllUsersAsync()
    {
        return await _mediator.Send(new GetUsersQuery());
    }

    public async Task<UserResponseDto> GetUserByIdAsync(int id)
    {
        return await _mediator.Send(new GetUserByIdQuery { UserId = id });
    }

    public async Task<List<UserResponseDto>> GetUsersByRoleIdAsync(int roleId)
    {
        return await _mediator.Send(new GetUsersByRoleIdQuery { RoleId = roleId });
    }

    public async Task<bool> UpdateUserDetailsAsync(UpdateUserRequestDto request)
    {
        var command = _mapper.Map<UpdateUserCommand>(request);
        return await _mediator.Send(command);
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        var command = new DeleteUserCommand { Id = id };
        return await _mediator.Send(command);
    }

}
