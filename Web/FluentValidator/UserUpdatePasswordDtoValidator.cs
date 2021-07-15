using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Web.DTO.User;

namespace Web.FluentValidator
{
    public class UserUpdatePasswordDtoValidator : AbstractValidator<UserUpdatePasswordDto>
    {
        public UserUpdatePasswordDtoValidator()
        {
            RuleFor(p => p.NewPassword).NotEmpty()
                .MinimumLength(4).MaximumLength(28);
        }
    }
}
