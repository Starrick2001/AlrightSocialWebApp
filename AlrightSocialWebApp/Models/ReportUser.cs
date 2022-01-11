using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AlrightSocialWebApp.Models
{
    public class ReportUser
    {
        [Required]
        public string UserEmail { get; set; }
        [Required]
        public string ReportedUser { get; set; }
        public string? Content { get; set; }
        public DateTime Time { get; set; }
    }
}
