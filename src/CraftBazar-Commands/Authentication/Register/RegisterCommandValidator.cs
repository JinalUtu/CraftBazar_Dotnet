using CraftBazar.Commands.Authentication.Register;
using FluentValidation;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.FirstName)
                    .NotEmpty()
                    .WithMessage("First Name is required.")
                    .MaximumLength(50)
                    .WithMessage("First Name cannot exceed 50 characters.");
        RuleFor(x => x.LastName)
                    .NotEmpty()
                    .WithMessage("Last Name is required.")
                    .MaximumLength(50)
                    .WithMessage("Last Name cannot exceed 50 characters.");
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty().MinimumLength(8);
        RuleFor(x => x.RoleId).GreaterThan(0);
    }
}