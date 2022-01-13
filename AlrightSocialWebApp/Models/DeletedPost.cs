using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AlrightSocialWebApp.Models
{
    public class DeletedPost
    {
        [Key]
        public int ID { set; get; }
        public String Title { get; set; }
        public String Content { get; set; }
        public DateTime? TimeCreate { get; set; }
        public DateTime? TimeModified { get; set; }
        public String Author { get; set; }
        public String Privacy { get; set; }
        public String ImageURL { get; set; }
    }
}
