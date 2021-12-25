using AlrightSocialWebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlrightSocialWebApp.Controllers
{
    public class SignInController : Controller
    {
        private DataContext db = new DataContext();

        public IActionResult Index()
        {
            return View("SignInGUI", new User());
        }
        [HttpPost]
        [Route("signin")]
        public IActionResult SignIn(string EmailAddress, string Password)
        {
            var account = checkAccount(EmailAddress, Password);
            if (account == null)
            {
                ViewBag.error = "Sai địa chỉ Email hoặc mật khẩu";
                return View("SignInGUI");
            }
            else
            {
                HttpContext.Session.SetString("email", EmailAddress);
                HttpContext.Session.SetString("name", account.name);
                HttpContext.Session.SetString("avatarUrl", account.AvatarURL);
                return RedirectToAction("Index", "HomePage");
            }
        }
        private User checkAccount(string Email, string Password)
        {
            var account = db.Users.SingleOrDefault(a => a.EmailAddress.Equals(Email));
            if (account != null)
            {
                if (BCrypt.Net.BCrypt.Verify(Password, account.Password))
                {
                    account.SignInStatus = "Online";
                    db.Users.Update(account);
                    db.SaveChanges();
                    return account;
                }
            }
            return null;
        }
    }
}
