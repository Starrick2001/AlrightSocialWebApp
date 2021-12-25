using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlrightSocialWebApp.Models;


namespace AlrightSocialWebApp.Controllers
{
    public class SignUpController : Controller
    {
        private DataContext db = new DataContext();
        public IActionResult Index()
        {
            return View("SignUpGUI", new User());
        }

        [HttpPost]
        [Route("signup")]
        public IActionResult SignUp(User account, string ReEnterPassword)
        {
            if (account.Password == ReEnterPassword)
            {
                var search = db.Users.SingleOrDefault(a => a.EmailAddress.Equals(account.EmailAddress));
                if (search != null)
                {
                    ViewBag.error = "Tài khoản đã tồn tại";
                    return View("SignUpGUI");
                }
                else
                {
                    account.Password = BCrypt.Net.BCrypt.HashPassword(account.Password);
                    account.AvatarURL = "/logo/logo_alrightsocial_circle.png";
                    db.Users.Add(account);
                    db.SaveChanges();
                    return RedirectToAction("Index", "SignIn");
                }

            }
            ViewBag.error = "Nhập lại mật khẩu không đúng";
            return View("SignUpGUI");
        }
    }
}
