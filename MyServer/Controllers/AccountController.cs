using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyServer.DTOs.Account;
using MyServer.Services;
using System.Security.Claims;

namespace MyServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : BaseCustomContoller
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
            var result = await _userAuthenticationService.CreateApplicationUserDtoAsync(model.UserName);
            return JsonOk(result);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            var message = await _userAuthenticationService.IsUserRegisteredAsync(model.Email, model.FirstName, model.LastName, model.Password);
            if (!String.IsNullOrEmpty(message))
            {
                return BadRequest(message);
            }

            return JsonOk("Ваш аккаунт успешно зарегестрирован!");
        }

    }
}
