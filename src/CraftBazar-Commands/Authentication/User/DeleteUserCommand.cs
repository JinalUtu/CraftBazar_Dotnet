using CraftBazar_DTO.Common;
using MediatR;

public class DeleteUserCommand : IRequest<bool>
{
    public int Id { get; set; }
}
