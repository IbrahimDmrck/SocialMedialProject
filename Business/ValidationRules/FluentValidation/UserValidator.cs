using Core.Entities.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ValidationRules.FluentValidation
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.FirstName).NotNull();
            RuleFor(x => x.FirstName).MinimumLength(3);
            RuleFor(x => x.FirstName).MaximumLength(50);

            RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.LastName).NotNull();
            RuleFor(x => x.LastName).MinimumLength(3);
            RuleFor(x => x.LastName).MaximumLength(50);

            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.Email).NotNull();
            RuleFor(x => x.Email).EmailAddress();
        }
    }
}
