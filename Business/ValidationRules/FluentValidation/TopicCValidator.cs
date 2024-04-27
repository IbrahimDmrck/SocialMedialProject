using Entities.Concrete;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation
{
    public class TopicCValidator:AbstractValidator<Topic>
    {
        public TopicCValidator()
        {
            RuleFor(x=>x.TopicTitle).NotEmpty();
            RuleFor(x=>x.TopicTitle).NotNull();
        }
    }
}
