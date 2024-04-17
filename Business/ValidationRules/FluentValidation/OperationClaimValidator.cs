using Core.Entities.Concrete;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation
{
    public class OperationClaimValidator:AbstractValidator<OperationClaim>
    {
        public OperationClaimValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Lütfen yetki adı giriniz");
            RuleFor(x => x.Name).NotNull().WithMessage("Lütfen yetki adı giriniz");
        }
    }
}
