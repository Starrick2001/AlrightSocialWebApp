using System;
using System.ComponentModel.DataAnnotations;

namespace AlrightSocialWebApp.Models
{
    public class Administrator
    {
        [Key]
        public string EmailAddress { get; set; }
        [Required]
        public string Password { get; set; }
        public string Name { get; set; }
        public string Sex { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string SignInStatus { get; set; }
        public string AvatarURL { get; set; }
    }
}
