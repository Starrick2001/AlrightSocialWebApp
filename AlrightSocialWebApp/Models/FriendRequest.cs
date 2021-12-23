using System;
using System.ComponentModel.DataAnnotations;

namespace AlrightSocialWebApp.Models
{
    public class FriendRequest
    {
        [Key]
        public string UserEmail { get; set; }
        [Key]
        public string FriendEmail { get; set; }
        public DateTime Time { get; set; }
    }
}
