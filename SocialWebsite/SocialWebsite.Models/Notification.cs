using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialWebsite.Models
{
    public class Notification
    {
        public int RecipientId { get; set; }
        public virtual User Recipient { get; set; } = null!;

        public int SenderId { get; set; }
        public virtual User Sender { get; set; } = null!;
    }
}
