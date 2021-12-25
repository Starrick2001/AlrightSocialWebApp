using System.ComponentModel.DataAnnotations;

namespace AlrightSocialWebApp.Models
{
    public class PostLike
    {
        [Key]
        public string UserEmail { get; set; }
        [Key]
        public int PostID { get; set; }
        public int? NotificationID { get; set; }

    }
}
