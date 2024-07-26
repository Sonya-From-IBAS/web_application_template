using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyServer.DTOs.Account;
using MyServer.Models;

namespace MyServer.Services
{
    public class UserAuthenticationService: IUserAuthenticationService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly JWTService _jwtService;

        public UserAuthenticationService(SignInManager<User> signInManager, UserManager<User> userManager, JWTService jwtService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _jwtService = jwtService;
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
                Email = email,
                EmailConfirmed = true
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
            return null;
        }

        public async Task<UserDto> RefreshUserTokenAsync(string userName)
        {
            return await CreateApplicationUserDtoAsync(userName);
        }
    }
}
