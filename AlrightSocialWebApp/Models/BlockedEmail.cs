using System.ComponentModel.DataAnnotations;

namespace AlrightSocialWebApp.Models
{
    public class BlockedEmail
    {
        [Required]
        public string UserEmail { get; set; }
        [Required]
        public string BlockedUser { get; set; }
    }
}
