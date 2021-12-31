using System.ComponentModel.DataAnnotations;

namespace AlrightSocialWebApp.Models
{
    public class SuspendedUser
    {
        [Key]
        public string SuspendedEmail { get; set; }
    }
}
