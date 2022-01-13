using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AlrightSocialWebApp.Models
{
    public class DeletedNotification
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string UserEmail { get; set; }
        public string Content { get; set; }
        public DateTime Time { get; set; }
        public bool IsRead { get; set; }
        public int? PostID { get; set; }
    }
}
