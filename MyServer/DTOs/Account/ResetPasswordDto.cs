using System.ComponentModel.DataAnnotations;

namespace MyServer.DTOs.Account
{
    public class ResetPasswordDto: ConfirmEmailDto
    {
        [Required]
        [StringLength(15, MinimumLength = 6, ErrorMessage ="New password must be at least{2} and maximum{1} characters")]
        public string NewPassord { get; set; }
    }
}
