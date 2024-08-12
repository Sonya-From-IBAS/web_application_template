using MyServer.DTOs.Account;

namespace MyServer.Services
{
    public interface IUserAuthenticationService
    {
        /// <summary>
        /// Проверка, зарегистрирован ли такой юзер
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns>вернет null, если все ок, иначе - строку с описанием ошибки</returns>
        public Task<string[]> IsUserUnauthorizedAsync(string userName, string password);
        
        /// <summary>
        /// Создает jwt токен для юзера
        /// </summary>
        /// <param name="userName"></param>
        /// <returns>имя + фамилия + токен</returns>
        public Task<UserDto> CreateApplicationUserDtoAsync(string userName);

        /// <summary>
        /// Регистрация пользователя
        /// </summary>
        /// <param name="email"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="password"></param>
        /// <returns>вернет null, если все ок, иначе - массив строк с описанием ошибок</returns>
        public Task<string[]> RegisterUserAsync(string email, string firstName, string lastName, string password);
        
        /// <summary>
        /// Обновление jwt токена
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public Task<UserDto> RefreshUserTokenAsync(string userName);

        /// <summary>
        /// Подтверждает почту пользователя
        /// </summary>
        /// <param name="userData"></param>
        /// <returns></returns>
        public Task<string> ConfirmEmailAsync(ConfirmEmailDto userData);
    }
}
