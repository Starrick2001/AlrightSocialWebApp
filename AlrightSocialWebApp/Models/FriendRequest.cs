using System;
using System.ComponentModel.DataAnnotations;

namespace AlrightSocialWebApp.Models
{
    public class FriendRequest
    {
        [Required]
        public string UserEmail { get; set; }
        [Required]
        public string FriendEmail { get; set; }
        public DateTime Time { get; set; }
    }
}
