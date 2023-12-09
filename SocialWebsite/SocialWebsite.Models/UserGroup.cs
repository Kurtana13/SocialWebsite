using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialWebsite.Models
{
    public class UserGroup
    {
        public UserGroup(int groupId,int userId)
        {
            this.GroupId = groupId;
            this.UserId = userId;
        }
        public int UserId { get; set; }
        public int GroupId { get; set; }

        public User User { get; set; } = null!;
        public Group Group { get; set; } = null!;
    }
}
