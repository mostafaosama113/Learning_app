using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Learning_platform.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string Image { get; set; }
    }
}
