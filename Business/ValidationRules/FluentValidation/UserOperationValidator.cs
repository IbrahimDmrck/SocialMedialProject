using Core.Entities.Concrete;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation
{
    public class UserOperationValidator :AbstractValidator<UserOperationClaim>
    {
        public UserOperationValidator()
        {
            RuleFor(x=>x.UserId).NotEmpty().NotNull();
            RuleFor(x=>x.OperationClaimId).NotEmpty().NotNull();
        }
    }
}
