using Core.Entities.Concrete;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation
{
    public class OperationCValidator : AbstractValidator<OperationClaim>
    {
        public OperationCValidator()
        {
            RuleFor(x=>x.Name).NotEmpty();
            RuleFor(x=>x.Name).NotNull();
        }
    }
}
