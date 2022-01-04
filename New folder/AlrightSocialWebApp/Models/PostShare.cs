using System;
using System.ComponentModel.DataAnnotations;

namespace AlrightSocialWebApp.Models
{
    public class PostShare
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
