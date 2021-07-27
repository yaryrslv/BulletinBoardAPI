using FluentValidation;
using Web.DTO.User;

namespace Web.FluentValidator
{
    public class UserUpdatePhoneNumberDtoValidator : AbstractValidator<UserUpdatePhoneNumberDto>
    {
        public UserUpdatePhoneNumberDtoValidator()
        {
            RuleFor(p => p.PhoneNumber).NotEmpty()
                .MinimumLength(4).MinimumLength(28);
        }
    }
}
