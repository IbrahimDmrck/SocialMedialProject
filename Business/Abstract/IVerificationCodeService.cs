using Core.Service;
using Core.Utilities.Result.Abstract;
using Core.Utilities.Result.Concrete;
using Entities.Concrete;
using Entities.DTOs;
using Entities.Models;

namespace Business.Abstract
{
    public interface IVerificationCodeService 
    {
        IResult SendVerifyCode(VerificationCodeDto verificationCode);
        IResult CheckVerifyCode(VerificationCodeDto verificationCode);
        IResult SendCodeForgotPassword(ResetPassword resetPassword);
        IResult CheckCodeForgotPassword(ResetPassword resetPassword);
        IResult DeleteVerifyCode(int userId);
    }
}
