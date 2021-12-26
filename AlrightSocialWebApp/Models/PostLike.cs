using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlrightSocialWebApp.Models
{
    public class PostLike
    {
        [Required]
        public string UserEmail { get; set; }
        [Required]
        public int PostID { get; set; }
        public int? NotificationID { get; set; }

    }
}
