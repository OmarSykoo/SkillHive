using System.Security.Cryptography.X509Certificates;
using FluentValidation;
using Modules.Users.Domain.ValueObjects;


namespace Modules.Users.Application.UseCases.CreateUser
{
    internal class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(x => x.FirstName)
               .NotEmpty().WithMessage("First name is required.")
               .MinimumLength(2).WithMessage("First name must be at least 3 characters long.")
               .MaximumLength(20).WithMessage("First name must not exceed 20 characters.");
            RuleFor(x => x.LastName)
               .NotEmpty().WithMessage("Last name is required.")
               .MinimumLength(2).WithMessage("Last name must be at least 3 characters long.")
               .MaximumLength(20).WithMessage("Last name must not exceed 20 characters.");
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Role).NotEmpty().Must(role => UserRole.IsValidRole(role)).WithMessage("Invalid role");
            RuleFor(x => x.city).NotEmpty();
            RuleFor(x => x.state).NotEmpty();
            RuleFor(x => x.PhoneNumber)
               .NotEmpty().WithMessage("Phone number is required.")
               .Matches(@"^(?:\+20|0)(10|11|12|15)\d{8}$")
               .WithMessage("Invalid Egyptian phone number. It must start with +20 or 0, followed by 10, 11, 12, or 15, and contain 8 more digits.");
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                .Matches(@"\d").WithMessage("Password must contain at least one digit.")
                .Matches(@"[@$!%*?&]").WithMessage("Password must contain at least one special character (@, $, !, %, *, ?, &).");
        }
    }

}

