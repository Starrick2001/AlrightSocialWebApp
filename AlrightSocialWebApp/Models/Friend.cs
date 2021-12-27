using System.ComponentModel.DataAnnotations;

namespace AlrightSocialWebApp.Models
{
    public class Friend
    {
        [Required]
        public string UserEmail { get; set; }
        [Required]
        public string FriendEmail { get; set; }
    }
}
