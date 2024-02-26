using Entities.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ValidationRules.FluentValidation
{
    public class ArticleValidator:AbstractValidator<Article>
    {
        public ArticleValidator()
        {
            RuleFor(x=>x.UserId).NotEmpty().WithMessage("Lütfen önce giriş yapın");
            RuleFor(x=>x.TopicId).NotEmpty().WithMessage("Lütfen bir konu başlığı seçin");
            RuleFor(x=>x.Content).NotNull().WithMessage("Lütfen içi boş bir paylaşım yapmayın");
            RuleFor(x=>x.Content).NotEmpty().WithMessage("Lütfen içi boş bir paylaşım yapmayın");
        }
    }
}
