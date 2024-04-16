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
        IResult SendVerificationCode(VerificationCodeDto verificationCode);
        IResult SendCodeForPasswordReset(ResetPassword resetPassword);
        IResult CheckVerifyCode(VerificationCodeDto verificationCode);
        IResult CheckCodeForPasswordReset(ResetPassword resetPassword);
        IResult DeleteVerifyCode(int userId);
    }
}
