using AlrightSocialWebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlrightSocialWebApp.Areas.Admin.Controllers
{
    public class SignInController : Controller
    {
        [Area("Admin")]
        public IActionResult SignInGUI()
        {
            return View();
        }
        private DataContext db = new DataContext();

        [Area("Admin")]
        [HttpPost("[action]")]
        public IActionResult SignInForAdmin(string EmailAddress, string Password)
        {
            var account = checkAccount(EmailAddress, Password);
            if (account == null)
            {
                ViewBag.error = "Sai mật khẩu";
                return View("SignInGUI");
            }
            else
            {
                HttpContext.Session.SetString("email", EmailAddress);
                HttpContext.Session.SetString("name", account.Name);
                HttpContext.Session.SetString("avatarUrl", account.AvatarURL);
                return RedirectToAction("Index", "Home");
            }
        }
        private Administrator checkAccount(string Email, string Password)
        {
            var account = db.Administrator.SingleOrDefault(a => a.EmailAddress.Equals(Email));
            if (account != null)
            {
                if (BCrypt.Net.BCrypt.Verify(Password, account.Password))
                {
                    account.SignInStatus = "Online";
                    db.Administrator.Update(account);
                    db.SaveChanges();
                    return account;
                }
            }
            return null;
        }
    }

    
}
