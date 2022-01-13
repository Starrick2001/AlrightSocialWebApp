using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AlrightSocialWebApp.Models
{
    public class DeletedShare
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string UserEmail { get; set; }
        [Required]
        public int PostID { get; set; }
        public string Privacy { get; set; }
        public int? NotificationID { get; set; }
        public DateTime? Time { get; set; }
    }
}
