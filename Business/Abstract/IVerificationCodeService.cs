using Core.Service;
using Core.Utilities.Result.Abstract;
using Entities.Concrete;
using Entities.DTOs;

namespace Business.Abstract
{
    public interface IVerificationCodeService 
    {
        IResult SendVerificationCode(VerificationCodeDto verificationCode);
        IResult CheckVerifyCode(int userId, string code);
        IResult DeleteVerifyCode(int userId);
    }
}
