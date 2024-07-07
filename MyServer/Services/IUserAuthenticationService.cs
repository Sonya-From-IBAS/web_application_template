using MyServer.DTOs.Account;
using MyServer.Models;

namespace MyServer.Services
{
    public interface IUserAuthenticationService
    {
        public Task<string> IsUserUnauthorizedAsync(string userName, string password);
        //public UserDto CreateApplicationUserDto(User user);
        public Task<UserDto> CreateApplicationUserDtoAsync(string userName);
        public Task<string> IsUserRegisteredAsync(string email, string firstName, string lastName, string password);
        public Task<UserDto> RefreshUserTokenAsync(string userName);
    }
}
