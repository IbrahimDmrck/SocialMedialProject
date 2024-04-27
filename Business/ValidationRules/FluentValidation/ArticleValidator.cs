using Entities.Concrete;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation
{
    public class ArticleValidator : AbstractValidator<Article>
    {
        public ArticleValidator()
        {
            RuleFor(x=>x.Content).NotEmpty().WithMessage("Lütfen bir içerik yazısı ekleyin !");
            RuleFor(x=>x.Content).NotNull().WithMessage("Lütfen bir içerik yazısı ekleyin !");
            RuleFor(x => x.TopicId).NotEmpty().WithMessage("Lütfen bir konu başlığı seçin !");
            RuleFor(x => x.UserId).NotNull().WithMessage("Lütfen önce giriş yapın !");
        }

    }
}
