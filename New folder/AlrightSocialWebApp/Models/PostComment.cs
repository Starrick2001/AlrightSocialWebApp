using System;
using System.ComponentModel.DataAnnotations;

namespace AlrightSocialWebApp.Models
{
    public class PostComment
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
