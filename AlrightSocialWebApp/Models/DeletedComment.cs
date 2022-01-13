using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AlrightSocialWebApp.Models
{
    public class DeletedComment
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string UserEmail { get; set; }
        [Required]
        public int PostID { get; set; }
        public string Content { get; set; }
        public int? NotificationID { get; set; }
        public DateTime? Time { get; set; }
    }
}
