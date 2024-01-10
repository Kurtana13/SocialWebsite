﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialWebsite.Models.ViewModels
{
    public class CommentViewModel
    {
        [Required]
        [DisplayName("Content")]
        public string Content { get; set; } = null!;
    }
}