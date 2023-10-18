using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialWebsite.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter text!")]
        public string? Title { get; set; }
        public string? Content { get; set; }

        [Display(Name = "User ID")]
        [Required(ErrorMessage = "Please enter user ID!")]
        public int UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual User? User { get; set; }

        public virtual ICollection<Comment>? Comments { get; set; }
    }
}
