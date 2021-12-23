using System;
using System.ComponentModel.DataAnnotations;

namespace AlrightSocialWebApp.Models
{
    public class CommentLike
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string SenderEmail { get; set; }
        [Required]
        public string ReceiverEmail { get; set; }
        public DateTime Time { get; set; }
        public string Content { get; set; }
    }
}
