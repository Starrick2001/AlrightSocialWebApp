using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AlrightSocialWebApp.Models
{
    public class PostReport
    {
        [Required]
        public string EmailAddress { get; set; }
        [Required]
        public int PostID { get; set; }
        public string? Content { get; set; }
        public DateTime Time { get; set; }
    }
}
