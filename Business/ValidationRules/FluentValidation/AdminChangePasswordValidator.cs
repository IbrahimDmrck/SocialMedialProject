using Entities.Concrete;
using Entities.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ValidationRules.FluentValidation
{
    public class AdminChangePasswordValidator:AbstractValidator<ChangePasswordModel>
    {
        public AdminChangePasswordValidator()
        {
            RuleFor(x=>x.Email).NotEmpty(); 
            RuleFor(x=>x.NewPassword).NotEmpty(); 
        }
    }
}
