using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SocialWebsite.Models
{
    public class Friend
    {
        public int UserId { get; set; }
        public virtual User _user { get; set; }
        
        public int FriendId { get; set; }
        public virtual User _friend { get; set; }
    }
}
