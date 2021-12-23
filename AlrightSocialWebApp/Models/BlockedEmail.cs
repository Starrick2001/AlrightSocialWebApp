using System.ComponentModel.DataAnnotations;

namespace AlrightSocialWebApp.Models
{
    public class BlockedEmail
    {
        [Key]
        public string UserEmail { get; set; }
        [Required]
        public string Blocked { get; set; }
    }
}
