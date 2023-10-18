using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SocialWebsite.Models
{
    public class Notification
    {
        public int RecipientId { get; set; }
        public virtual User Recipient { get; set; }

        public int SenderId { get; set; }
        public virtual User Sender { get; set; }
    }
}
