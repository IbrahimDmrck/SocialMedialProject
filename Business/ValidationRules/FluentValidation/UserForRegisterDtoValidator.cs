using Entities.DTOs;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation
{
    public class UserForRegisterDtoValidator : AbstractValidator<UserForRegisterDto>
    {
        public UserForRegisterDtoValidator()
        {
            RuleFor(x => x.Email).NotEmpty().NotNull().EmailAddress().WithMessage("Lütfen geçerli bir e-posta adresi yazınız");
            RuleFor(x => x.Password).NotEmpty().NotNull().MinimumLength(8).WithMessage("Lütfen şifrenizi en az 8 karakter uzunluğunda yazınız");

            RuleFor(x=>x.FirstName).NotNull().MinimumLength(2).WithMessage("Adınız boş bırakılamaz ve en az 2 karakter olmalı");
            RuleFor(x=>x.LastName).NotNull().MinimumLength(2).WithMessage("Soyadınız boş bırakılamaz ve en az 2 karakter olmalı");

            RuleFor(x=>x.Gender).NotEmpty().NotNull().WithMessage("Lütfen cinsiyetinizi seçiniz");
            RuleFor(x=>x.PhoneNumber).NotEmpty().NotNull().WithMessage("Lütfen telefon numaranızı yazınız");




        }
    }
}
