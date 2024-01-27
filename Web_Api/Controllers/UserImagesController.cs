using Business.Abstract;
using Core.Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Web_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserImagesController : ControllerBase
    {
        private readonly IUserImageService _userImageService;

        public UserImagesController(IUserImageService userImageService)
        {
            _userImageService = userImageService;
        }

        [HttpGet("getall")]
        public IActionResult GetAll()
        {
            var result = _userImageService.GetAll();
            if (result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("getallbyuserid")]
        public IActionResult GetAllByUserId(int userId)
        {
            var result = _userImageService.GetUserImages(userId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("add")]
        public IActionResult Add([FromForm] int userId, [FromForm] IFormFile userImage)
        {
            var result = _userImageService.Add(userImage, userId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("update")]
        public IActionResult Update([FromForm] UserImage userImage, [FromForm] IFormFile imageFile)
        {

            var result = _userImageService.Update(userImage, imageFile);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }



        [HttpDelete("delete")]
        public IActionResult Delete(UserImage userImage)
        {
            var result = _userImageService.Delete(userImage);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }

}
