using System.ComponentModel.DataAnnotations;

namespace AlrightSocialWebApp.Models
{
    public class CommentLike
    {
        [Key]
        public string UserEmail { get; set; }
        [Required]
        public int CmtID { get; set; }
        public int NotificationID { get; set; }
    }
}
