using FluentValidation;
using Web.DTO.User;

namespace Web.FluentValidator
{
    public class UserRegisterDtoValidator : AbstractValidator<UserRegisterDto>
    {
        public UserRegisterDtoValidator()
        {
            RuleFor(p => p.Email).NotEmpty().EmailAddress()
                .WithErrorCode("Invalid Email address");
            RuleFor(p => p.Password).NotEmpty().MinimumLength(4).MaximumLength(28)
                .WithErrorCode("Invalid password lenght");
            RuleFor(p => p.PhoneNumber).NotEmpty()
                .WithErrorCode("Invalid phone lenght");
            RuleFor(p => p.Username).NotEmpty().MinimumLength(4).MaximumLength(28)
                .WithErrorCode("Invalid username lenght");
        }
    }
}
