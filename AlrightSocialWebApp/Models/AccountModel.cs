using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AlrightSocialWebApp.Models
{
    public class AccountModel
    {
        [Required(ErrorMessage = "Email không được để trống")]
        [Key]
        public string Email { set; get; }
        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        public string Password { set; get; }
    }
}
