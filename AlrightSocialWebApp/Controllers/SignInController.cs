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
                return View("~/Views/Home/Index.cshtml");
            }
        }
        private User checkAccount(string Email, string Password)
        {
            var account = db.Users.SingleOrDefault(a => a.EmailAddress.Equals(Email));
            if (account != null)
            {
                if (BCrypt.Net.BCrypt.Verify(Password, account.Password))
                {
                    return account;
                }
            }
            return null;
        }
    }
}
