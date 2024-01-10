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
    public class Post
    {
        public Post() { }
        public Post(PostViewModel postViewModel)
        {
            this.Title = postViewModel.Title;
            this.Content = postViewModel.Content;
        }
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;

        public int? GroupId { get; set; }
        public virtual Group? Group { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; } = null!;

        public virtual ICollection<Comment>? Comments { get; set; }
    }
}
