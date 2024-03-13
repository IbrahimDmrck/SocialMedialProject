using Core.Service;
using Core.Utilities.Result.Abstract;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface IVerificationCodeService 
    {
        IResult SendVerificationCode(int userId, string Email);

        IResult DeleteVerifyCode(int userId);
    }
}
