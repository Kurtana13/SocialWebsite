using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Collections.ObjectModel;

namespace SocialWebsite.Models
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; } = null;

        public virtual ICollection<UserGroup>? UserGroups { get; set; }
        public virtual ICollection<Post>? Posts { get; set; }

    }
}
