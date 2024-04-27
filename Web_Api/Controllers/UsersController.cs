using Business.Abstract;
using Core.Entities.Concrete;
using Core.Utilities.Result.Abstract;
using Entities.DTOs;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using IResult = Core.Utilities.Result.Abstract.IResult;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("getall")]
        public ActionResult GetAll()
        {
            IDataResult<List<User>> result = _userService.GetAll();
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("getalldto")]
        public ActionResult GetAllDto()
        {
            IDataResult<List<UserDto>> result = _userService.GetAllDto();
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("getbyid")]
        public IActionResult GetById(int id)
        {
            IDataResult<UserDto> result = _userService.GetUserDtoById(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("add")]
        public IActionResult Add(User user)
        {
            IResult result = _userService.Add(user);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("delete")]
        public IActionResult Delete([FromQuery] int id)
        {
            IResult result = _userService.DeleteById(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("update")]
        public IActionResult Update(UserDto user)
        {
            IResult result = _userService.UpdateByDto(user);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
