using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Collections.ObjectModel;
using SocialWebsite.Models.ViewModels;

namespace SocialWebsite.Models
{
    public class Group
    {
        public Group() { }
        public Group(GroupViewModel groupViewModel)
        {
            this.Name = groupViewModel.Name;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string CreatorUsername { get; set; }

        public virtual ICollection<UserGroup>? UserGroups { get; set; }
        public virtual ICollection<Post>? Posts { get; set; }

    }
}
