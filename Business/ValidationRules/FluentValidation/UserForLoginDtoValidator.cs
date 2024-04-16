using Entities.DTOs;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation
{
    public class UserForLoginDtoValidator : AbstractValidator<UserForLoginDto>
    {
        public UserForLoginDtoValidator()
        {
            RuleFor(x => x.Email).NotEmpty().NotNull().EmailAddress().WithMessage("Lütfen geçerli bir e-posta adresi yazınız");
            RuleFor(x => x.Password).NotEmpty().NotNull().MinimumLength(8).WithMessage("Lütfen şifrenizi en az 8 karakter uzunluğunda yazınız");
        }
    }
}
