using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyServer.DTOs.Account;
using MyServer.Models;
using MyServer.Services;
using System.Security.Claims;

namespace MyServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserAuthenticationService _userAuthenticationService;

        public AccountController(IUserAuthenticationService userAuthenticationService)
        {
            _userAuthenticationService = userAuthenticationService;
        }

        [Authorize]
        [HttpGet("refresh-user-token")]
        public async Task<ActionResult<UserDto>> RefreshUserToken()
        {
            return await _userAuthenticationService.RefreshUserTokenAsync(User.FindFirst(ClaimTypes.Email)?.Value);
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto model)
        {
            var message = await _userAuthenticationService.IsUserUnauthorizedAsync(model.UserName, model.Password);

            if (!String.IsNullOrEmpty(message))  
            {
                return Unauthorized(message);
            }

            return await _userAuthenticationService.CreateApplicationUserDtoAsync(model.UserName);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto model) 
        {
            var message = await _userAuthenticationService.IsUserRegisteredAsync(model.Email, model.FirstName, model.LastName, model.Password);
            if (!String.IsNullOrEmpty(message))
            {
                return BadRequest(message);
            }

            return Ok(new JsonResult(new { text = "Ваш аккаунт успешно зарегестрирован!"}) );
        }

    }
}
