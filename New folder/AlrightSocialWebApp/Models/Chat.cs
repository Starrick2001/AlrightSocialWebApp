using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AlrightSocialWebApp.Models
{
    public class Chat
    {
        public Chat()
        {
            Messages = new List<Message>();
        }
        [Key]
        [Display(Name = "ID")]
        public int Id { get; set; }
        public ICollection<Message> Messages {get;set;}
        public string User1 { get; set; }
        public string User2 { get; set; }
    }
}
