using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SocialWebsite.Models
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; } = null;

        public virtual ICollection<User>? Users { get; set;}
        public virtual ICollection<Post>? Posts { get; set; }

    }
}
