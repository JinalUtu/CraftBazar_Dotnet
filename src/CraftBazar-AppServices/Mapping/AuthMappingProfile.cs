using AutoMapper;
using CraftBazar.Commands.Authentication.Login;
using CraftBazar.Commands.Authentication.Register;
using CraftBazar_DTO.Users;

public class AuthMappingProfile : Profile
{
    public AuthMappingProfile()
    {
        CreateMap<RegisterRequestDto, RegisterCommand>();
        CreateMap<LoginRequestDto, LoginCommand>();
        CreateMap<UpdateUserRequestDto, UpdateUserCommand>();
    }
}