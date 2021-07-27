using System;
using FluentValidation;
using Web.DTO.Ad;

namespace Web.FluentValidator
{
    public class AdDtoValidator : AbstractValidator<AdDto>
    {
        public AdDtoValidator()
        {
            RuleFor(p => p.City).NotEmpty().WithMessage("City are required");
            RuleFor(p => p.ImageUrl).Must(LinkMustBeAUri)
                .WithMessage("Link must be a valid URL");
            RuleFor(p => p.Text).NotEmpty().MaximumLength(2048);
        }
        private static bool LinkMustBeAUri(string imageUrl)
        {
            if (string.IsNullOrWhiteSpace(imageUrl))
            {
                return false;
            }

            return Uri.TryCreate(imageUrl, UriKind.RelativeOrAbsolute, out _);
        }
    }
}
