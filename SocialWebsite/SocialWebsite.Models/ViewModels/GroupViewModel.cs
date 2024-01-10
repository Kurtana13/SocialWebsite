using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialWebsite.Models.ViewModels
{
    public class GroupViewModel
    {
        [Required]
        [DisplayName("Name")]
        public string Name { get; set; } = null!;
    }
}
