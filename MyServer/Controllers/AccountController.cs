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
        //private readonly IEmailService _emailService;
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
            string[] messages = await _userAuthenticationService.IsUserUnauthorizedAsync(model.UserName, model.Password);

            if (messages != null)
            {
                return Unauthorized(messages);
            }
            var result = await _userAuthenticationService.CreateApplicationUserDtoAsync(model.UserName);
            return JsonOk(result);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            string[] messages = await _userAuthenticationService.RegisterUserAsync(model.Email, model.FirstName, model.LastName, model.Password);
            if (messages != null)
            {
                return BadRequest(messages);
            }

            return JsonOk("Ваш аккаунт успешно зарегестрирован! Пожалуйста, подтвердите ваш Email!");
        }

        [HttpPut("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] ConfirmEmailDto dto)
        {
            //var dto = new ConfirmEmailDto { Email = email, Token = token };
            string message = await _userAuthenticationService.ConfirmEmailAsync(dto);
            if (!String.IsNullOrEmpty(message))
            {
                return BadRequest(message);
            }
            return JsonOk("Email успешно подтвержден!");
        }

        [HttpPost("resend-email-confirmation-link/{email}")]
        public async Task<IActionResult> ResendEmailConfirmationLink(string email)
        {
            string message = await _userAuthenticationService.ResendEmailConfirmationLinkAsync(email);
            if (!String.IsNullOrEmpty(message))
            {
                return BadRequest(message);
            }
            return JsonOk("Сообщение отправлено повторно. Пожалуйста, подтвердите ваш Email!");
            
        }

        [HttpGet("try-it-out")]
        public string TryItOut()
        {
            return "trt it out!";
        }
    }
}
