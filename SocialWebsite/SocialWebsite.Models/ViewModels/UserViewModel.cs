using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialWebsite.Models.ViewModels
{
    public class UserViewModel
    {
        [Required]
        [DisplayName("Username")]
        public string Username { get; set; } = null!;
        [Required]
        [EmailAddress]
        [DisplayName("Email")]
        public string? Email { get; set; }
        [Required]
        [DisplayName("Password")]
        public string? Password { get; set; }
        [Required]
        [DisplayName("Confirm Password")]
        [Compare("Password", ErrorMessage = "Passwords don't match!")]
        public required string? ConfirmPassword { get; set; }

    }
}
