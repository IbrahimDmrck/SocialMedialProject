using Entities.DTOs;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation
{
    public class UserForLoginDtoValidator : AbstractValidator<UserForLoginDto>
    {
        public UserForLoginDtoValidator()
        {
            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.Email).NotNull();
            RuleFor(x => x.Email).EmailAddress();

            RuleFor(x => x.Password).NotEmpty();
            RuleFor(x => x.Password).NotNull();
            RuleFor(x => x.Password).MinimumLength(8);
            RuleFor(x => x.Password).MaximumLength(25);


        }
    }
}
