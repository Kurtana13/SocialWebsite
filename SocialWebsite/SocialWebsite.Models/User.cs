﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace SocialWebsite.Models
{
    public class User : IdentityUser<int>
    {
        public virtual ICollection<Post>? Posts { get; set; }   
        public virtual ICollection<UserGroup>? UserGroups { get; set; }
        public virtual ICollection<Friend>? Friends { get; set; }
        public virtual ICollection<Comment>? Comments { get; set; }
    }
}
