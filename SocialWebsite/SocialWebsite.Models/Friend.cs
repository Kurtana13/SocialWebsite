using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SocialWebsite.Models
{
    public class Friend
    {
        public int UserId { get; set; }
        public virtual User User { get; set; } = null!;

        public int FriendId { get; set; }
        public virtual User FriendUser { get; set; } = null!;
    }
}
