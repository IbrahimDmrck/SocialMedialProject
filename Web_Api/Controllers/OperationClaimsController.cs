using Business.Abstract;
using Core.Entities.Concrete;
using Core.Utilities.Result.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using IResult = Core.Utilities.Result.Abstract.IResult;
namespace Web_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperationClaimsController : ControllerBase
    {
        private readonly IOperationClaimService _operationClaimService;

        public OperationClaimsController(IOperationClaimService operationClaimService)
        {
            _operationClaimService = operationClaimService;
        }

        [HttpGet("getall")]
        public ActionResult GetAll()
        {
            IDataResult<List<OperationClaim>> operationClaims = _operationClaimService.GetAll();
            return operationClaims.Success ? Ok(operationClaims) : BadRequest(operationClaims);
        }

        [HttpGet("getbyid")]
        public ActionResult GetById(int id)
        {
            IDataResult<OperationClaim> operationClaim = _operationClaimService.GetEntityById(id);
            return operationClaim.Success ? Ok(operationClaim) : BadRequest(operationClaim);
        }

        [HttpPost("add")]
        public IActionResult Add(OperationClaim operationClaim)
        {
            IResult result = _operationClaimService.Add(operationClaim);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("delete")]
        public IActionResult Delete(int id)
        {
            IResult result = _operationClaimService.Delete(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("update")]
        public IActionResult Update(OperationClaim operationClaim)
        {
            IResult result = _operationClaimService.Update(operationClaim);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
