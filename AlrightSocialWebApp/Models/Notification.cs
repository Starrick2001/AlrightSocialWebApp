using System;
using System.ComponentModel.DataAnnotations;

namespace AlrightSocialWebApp.Models
{
    public class Notification
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string UserEmail { get; set; }
        public string Content { get; set; }
        public DateTime Time { get; set; }
        public bool IsRead { get; set; }
        public int? PostID { get; set; }
    }
}
