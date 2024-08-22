using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using MyServer.DTOs.Account;
using MyServer.Models;
using System.Text;

namespace MyServer.Services
{
    public class UserAuthenticationService: IUserAuthenticationService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly JWTService _jwtService;
        private readonly EmailService _emailService;
        private readonly IConfiguration _configuration;

        public UserAuthenticationService(
            UserManager<User> userManager, 
            SignInManager<User> signInManager, 
            JWTService jwtService,
            IConfiguration configuration, 
            EmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtService = jwtService;
            _configuration = configuration;
            _emailService = emailService;
        }

        public async Task<string[]> IsUserUnauthorizedAsync(string userName, string password)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return ["Неверное имя пользователя или пароль!"];
            }

            if (user.EmailConfirmed == false)
            {
                return ["Пожалуйста, подтвердите свой email!"];
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);

            if (!result.Succeeded)
            {
                return ["Неверное имя пользователя или пароль!"];
            }
            

            return null;
        }

        public async Task<UserDto> CreateApplicationUserDtoAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);

            return new UserDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                JWT = _jwtService.CreateJWT(user)
            };
        }

        public async Task<string[]> RegisterUserAsync(string email, string firstName, string lastName, string password)
        {
            if(await _userManager.Users.AnyAsync(x => x.Email == email))
            {
                return ["Пользователь с таким email уже существует!"];
            }
            var user = new User
            {
                FirstName = firstName.ToLower(),
                LastName = lastName.ToLower(),
                UserName = email,
                Email = email
            };
            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded) 
            {
                var errors = result.Errors.ToList();
                var resultMessage = new string[errors.Count];
                for(int i = 0; i < errors.Count; i++)
                {
                    resultMessage[i] = errors[i].Description;
                }

                return resultMessage;
            }
            try
            {
                if(await SendConfirmEmailAsync(user))
                {
                    return null;
                }
                return ["Failed to send email!"];
            }
            catch (Exception)
            {
                return ["Failed to send email!"];
            }
        }

        public async Task<UserDto> RefreshUserTokenAsync(string userName)
        {
            return await CreateApplicationUserDtoAsync(userName);
        }

        private async Task<bool> SendConfirmEmailAsync(User user)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            var url = GenerateUrlLinkForEmail("Email:ConfirmEmailPath", token, user.Email);
            var body = $"<p>Hello: {user.FirstName} {user.LastName}</p>" +
                "<p>Please, confirm your email address by clicking on the following link</p>" +
                $"<p><a href=\"{url}\">Click here</a></p>" + 
                "<br>Leon Application";
            var mailData = new MailData([user.Email], "Confirm your email", body);
            return await _emailService.SendEmailAsync(mailData);
        }

        private async Task<bool> SendForgotUsernameOrPasswordEmail(User user)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            
            var url = GenerateUrlLinkForEmail("Email:ResetPasswordPath", token, user.Email);

            var body = $"<p>Hello: {user.FirstName} {user.LastName}</p>" +
                $"<p>Username: {user.UserName}</p>" +
                $"<p>To reset your password, please click on the linkL</p>" +
                $"<p><a href=\"{url}\">Click here</a></p>" +
                "<br>Leon Application";
            var mailData = new MailData([user.Email], "Reset your password", body);
            return await _emailService.SendEmailAsync(mailData);
        }

        private string GenerateUrlLinkForEmail(string action, string token, string email)
        {
            return $"{_configuration["JWT:Issuer"]}/api/{_configuration[action]}?Token={token}&Email={email}";
        }

        public async Task<string> ConfirmEmailAsync(ConfirmEmailDto userData)
        {
            var user = await _userManager.FindByEmailAsync(userData.Email);
            if (user == null) return "Email address has not been registered!";
            if (user.EmailConfirmed == true) return "Your email alredy has been confirmed!";
            try
            {
                string token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(userData.Token));
                //Помимо подтверждения проходит проверка токена
                var result = await _userManager.ConfirmEmailAsync(user, token);
                if(result.Succeeded)
                {
                    return null;
                }
                return "Invalid token. Please, try again!";
            }
            catch (Exception)
            {
                return "Invalid token. Please, try again!";
            }
        }

        public async Task<string> ResendEmailConfirmationLinkAsync(string email)
        {
            if (String.IsNullOrEmpty(email)) 
                return "Invalid email";

            User user = await _userManager.FindByEmailAsync(email);

            if (user == null)
                return "This email has not been registered yet!";

            if (user.EmailConfirmed == true)
                return "Your email address already has been confirmed. You can login to your account";

            try
            {
                if (await SendConfirmEmailAsync(user))
                    return null;
                return "Failed to send email";
            }
            catch (Exception ex)
            {
                return $"Failed to send email: {ex.GetBaseException().Message}";
            }
        }

        public async Task<string> ForgotUsernameOrPassord(string email)
        {
            if (String.IsNullOrEmpty(email))
            {
                return "Invalid Email!";
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return "This email address has not been registered";
            }

            try
            {
                if (await SendForgotUsernameOrPasswordEmail(user))
                {
                    return null;
                }

                return "Failed to send message on email!";
            }
            catch (Exception ex)
            {
                return "Failed to send message on email!";
            }
        }

        public async Task<string> ResetPassword(ResetPasswordDto resetPassword)
        {
            var user = await _userManager.FindByEmailAsync(resetPassword.Email);
            if (user == null)
                return "This email has not been registered yet!";

            if (user.EmailConfirmed == true)
                return "Your email address already has been confirmed. You can login to your account";

            try
            {
                string token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(resetPassword.Token));
                //Помимо подтверждения проходит проверка токена
                var result = await _userManager.ResetPasswordAsync(user, token, resetPassword.NewPassord);
                if (result.Succeeded)
                {
                    return null;
                }
                return "Invalid token. Please, try again!";
            }
            catch (Exception)
            {
                return "Invalid token. Please, try again!";
            }
        }

    }
}
