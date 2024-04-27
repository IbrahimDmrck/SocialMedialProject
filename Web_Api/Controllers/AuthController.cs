using Business.Abstract;
using Business.Concrete;
using Entities.DTOs;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public IActionResult Login(UserForLoginDto userForLoginDto)
        {
            var userToLogin = _authService.Login(userForLoginDto);
            if (!userToLogin.Success)
            {
                return BadRequest(userToLogin);
            }

            var result = _authService.CreateAccessToken(userToLogin.Data);

            return result.Success ? Ok(result) : BadRequest(result);
        }
        [HttpPost("register")]
        public IActionResult Register(UserForRegisterDto userForRegisterDto)
        {
            var userExists = _authService.UserExist(userForRegisterDto.Email);
            if (!userExists.Success)
            {
                return BadRequest(userExists);
            }

            var registerResult = _authService.Register(userForRegisterDto, userForRegisterDto.Password);
            var result = _authService.CreateAccessToken(registerResult.Data);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("changepassword")]
        public IActionResult ChangePassword(ChangePasswordModel changePassword)
        {
            var result = _authService.ChangePassword(changePassword);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("adminchangepassword")]
        public IActionResult AdminChangePassword(ChangePasswordModel changePassword)
        {
            var result = _authService.AdminChangePassword(changePassword);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}