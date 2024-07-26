using System.ComponentModel.DataAnnotations;

namespace MyServer.DTOs.Account
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "First name is required")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Имя должно содержать минимум {2} и максимум {1} символов")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last name is required")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Фамилия должна содержать минимум {2} и максимум {1} символов")]    
        public string LastName { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [RegularExpression("^\\w+@[a-zA-Z_]+?\\.[a-zA-Z]{2,3}$", ErrorMessage = "Некорректный email адрес")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Пароль должнен содержать минимум {2} и максимум {1} символов")]
        public string Password { get; set; }
    }
}
