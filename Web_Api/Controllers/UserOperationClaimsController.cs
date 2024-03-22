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
        public ActionResult GetAll()
        {
            IDataResult<List<UserOperationClaim>> operationClaims = _userOperationClaimService.GetAll();
            return operationClaims.Success ? Ok(operationClaims) : BadRequest(operationClaims);
        }

        [HttpPost("add")]
        public IActionResult Add(UserOperationClaim operationClaim)
        {
            IResult result = _userOperationClaimService.Add(operationClaim);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("delete")]
        public IActionResult Delete(int userId, int claimId)
        {
            IResult result = _userOperationClaimService.Delete(userId, claimId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("update")]
        public IActionResult Update(UserOperationClaim operationClaim)
        {
            IResult result = _userOperationClaimService.Update(operationClaim);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
