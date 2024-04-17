using Core.Entities.Concrete;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("Lütfen adınızı yazın");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Lütfen soyadınızı yazın");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Lütfen e-posta adresinizi yazın");
            RuleFor(x => x.Email).EmailAddress().WithMessage("Lütfen geçerli bir e-posta adresi yazın");
            RuleFor(x => x.Gender).NotEmpty().WithMessage("Lütfen cinsiyetinizi seçin");
            RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("Lütfen telefon numaranızı yazın");
        }
    }
}
