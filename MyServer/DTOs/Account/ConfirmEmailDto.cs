using System.ComponentModel.DataAnnotations;

namespace MyServer.DTOs.Account
{
    public class ConfirmEmailDto
    {
        /// <summary>
        /// Email сonfirmation токен
        /// </summary>
        [Required(ErrorMessage = "Token is required")]
        public string Token { get; set; }

        /// <summary>
        /// имэйл
        /// </summary>
        [Required(ErrorMessage = "Email is required")]
        [RegularExpression("^\\w+@[a-zA-Z_]+?\\.[a-zA-Z]{2,3}$", ErrorMessage = "Некорректный email адрес")]
        public string Email { get; set; }
    }
}
