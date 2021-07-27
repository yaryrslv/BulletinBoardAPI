using FluentValidation;
using Web.DTO.User;

namespace Web.FluentValidator
{
    public class UserUpdateEmailDtoValidator : AbstractValidator<UserUpdateEmailDto>
    {
        public UserUpdateEmailDtoValidator()
        {
            RuleFor(p => p.Email).NotEmpty().EmailAddress();
        }
    }
}
