using Business.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VerificationCodesController : ControllerBase
    {
        IVerificationCodeService _verificationCodeService;

        public VerificationCodesController(IVerificationCodeService verificationCodeService)
        {
            _verificationCodeService = verificationCodeService;
        }

        [HttpPost("sendcode")]
        public IActionResult SendVerificationCode(int userId, string Email)
        {
            var result = _verificationCodeService.SendVerificationCode(userId, Email);

            return result.Success ? Ok(result) : BadRequest(result);
        }


        [HttpPost("deleteverifycode")]
        public IActionResult DeleteVerifyCode(int userId)
        {
            var result = _verificationCodeService.DeleteVerifyCode(userId);

            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
