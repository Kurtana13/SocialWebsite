using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SocialWebsite.Models.ViewModels;

namespace SocialWebsite.Models
{
    public class Comment
    {
        public Comment() { }
        public Comment(CommentViewModel commentViewModel)
        {
            this.Content = commentViewModel.Content;
        }

        public int Id { get; set; }

        public string? Content { get; set; }

        public int PostId { get; set; }
        public virtual Post Post { get; set; } = null!;

        public int UserId { get; set; }
        public virtual User User { get; set; } = null!;
    }
}
