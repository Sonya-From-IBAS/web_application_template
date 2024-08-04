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
            string[] messages = await _userAuthenticationService.IsUserRegisteredAsync(model.Email, model.FirstName, model.LastName, model.Password);
            if (messages != null)
            {
                return BadRequest(messages);
            }

            return JsonOk("Ваш аккаунт успешно зарегестрирован!");
        }


        //[HttpGet("send-email")]
        //public async Task<IActionResult> SendMailAsync(MailData mailData)
        //{
        //    bool result = await _emailService.SendEmailAsync(mailData);
        //    return result
        //        ? JsonOk("Mail has successfully been sent")
        //        : JsonError("An error occured. The mail could not be sent");
            /*
             Test json object:

                {
                  "to": [
                    "LeonidPakhomov2003g@yandex.ru"
                  ],
                  "bcc": [
                    "Anevskyi2018@yandex.ru"
                  ],
                  "cc": [
                    "Anevskyi2018@yandex.ru"
                  ],
                  "from": "Anevskyi2018@yandex.ru",
                  "displayName": "Ht",
                  "replyTo": "",
                  "replyToName": "string",
                  "subject": "Attempt to authorize with email",
                  "body": "Test message, here will be a link to authorize!"
                }

            */
        //}

    }
}
