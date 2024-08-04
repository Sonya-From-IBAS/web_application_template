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

        //public UserDto CreateApplicationUserDto(User user)
        //{
        //    return new UserDto
        //    {
        //        FirstName = user.FirstName,
        //        LastName = user.LastName,
        //        JWT = _jwtService.CreateJWT(user)
        //    };
        //}

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

        public async Task<string[]> IsUserRegisteredAsync(string email, string firstName, string lastName, string password)
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
            var url = $"{_configuration["JWT:ClientUrl"]}/{_configuration["Email:ConfirmEmailPath"]}?token={token}&email={user.Email}";
            var body = $"<p>Hello: {user.FirstName} {user.LastName}</p>" +
                "<p>Please, confirm your email address by clicking on the following link</p>" +
                $"<p><a href=\"{url}\">Click here</a></p>" + 
                "<br>Leon Application";
            var mailData = new MailData([user.Email], "Confirm your email", body);
            return await _emailService.SendEmailAsync(mailData);
        }
    }
}
