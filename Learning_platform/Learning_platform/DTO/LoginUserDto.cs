using System.ComponentModel.DataAnnotations;

namespace Learning_platform.DTO
{
    public class LoginUserDto
    {
        [Required]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
