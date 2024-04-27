using Business.Abstract;
using Core.Entities.Concrete;
using Core.Utilities.Result.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using IResult = Core.Utilities.Result.Abstract.IResult;

namespace Web_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserOperationClaimsController : ControllerBase
    {
        private readonly IUserOperationClaimService _userOperationClaimService;

        public UserOperationClaimsController(IUserOperationClaimService userOperationClaimService)
        {
            _userOperationClaimService = userOperationClaimService;
        }

        [HttpGet("getall")]
        public IActionResult GetAll()
        {
            var result = _userOperationClaimService.GetAll();
            return result.Success ? Ok(result) : BadRequest(result);
        }


        [HttpPost("add")]
        public IActionResult Add(UserOperationClaim useroperationClaim)
        {
            var result = _userOperationClaimService.Add(useroperationClaim);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("delete")]
        public IActionResult Delete(int userId, int claimId)
        {
            var result = _userOperationClaimService.Delete(userId, claimId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("update")]
        public IActionResult Update(UserOperationClaim useroperationClaim)
        {
            var result = _userOperationClaimService.Update(useroperationClaim);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
