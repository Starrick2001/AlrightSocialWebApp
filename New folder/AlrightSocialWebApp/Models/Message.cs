using System;
using System.ComponentModel.DataAnnotations;

namespace AlrightSocialWebApp.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string SenderEmail { get; set; }
        public DateTime Time { get; set; }
        public string Content { get; set; }
        public int ChatId { get; set; }
        public Chat Chat { get; set; }
    }
}
