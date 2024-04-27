using Entities.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ValidationRules.FluentValidation
{
    public class CommentValidator:AbstractValidator<Comment>
    {
        public CommentValidator()
        {
            RuleFor(x => x.UserId).NotNull();
            RuleFor(x => x.ArticleId).NotNull();
            RuleFor(x => x.CommentText).NotNull();
            RuleFor(x => x.CommentText).NotEmpty();
        }
    }
}
