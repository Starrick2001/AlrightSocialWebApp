using System.ComponentModel.DataAnnotations;

namespace AlrightSocialWebApp.Models
{
    public class Friend
    {
        [Key]
        public string UserEmail { get; set; }
        [Key]
        public string FriendEmail { get; set; }
    }
}
