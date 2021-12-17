using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AlrightSocialWebApp.Models
{
    public class User
    {
        [Required(ErrorMessage = "Email không được để trống")]
        [Key]
        public string EmailAddress { set; get; }
        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        public string Password { set; get; }

        public string name { set; get; }
        public string sex { set; get; }
        public DateTime DateOfBirth { set; get; }
        public string PhoneNumber { set; get; }
        public string SignInStatus { set; get; }
        public string AvatarURL { set; get; }
    }
}
