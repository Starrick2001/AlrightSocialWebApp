using AlrightSocialWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Dynamic;

namespace AlrightSocialWebApp.Controllers
{
    public class ChangePasswordController : Controller
    {
        DataContext db = new DataContext();
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult ChangePasswordGUI(string EmailAddress)
        {
            User user = db.GetUserInfo(EmailAddress);
            dynamic mymodel = new ExpandoObject();
            mymodel.User = user;
            mymodel.Friends = db.GetListOfFriends(HttpContext.Session.GetString("email"));
            return View(mymodel);
        }

        [HttpPost]
        public IActionResult ChangePassword(string Email, string Password, string NewPassword, string ReEnterPassword)
        {
            var account = checkAccount(Email, Password);
            if (account == null)
            {
                TempData["error"] = "Sai mật khẩu";
                return RedirectToAction("ChangePasswordGUI", "ChangePassword", new { EmailAddress = HttpContext.Session.GetString("email") });
            }
            else
            {
                if (NewPassword != ReEnterPassword)
                {
                    TempData["error"] = "Nhập lại mật khẩu không chính xác";
                    return RedirectToAction("ChangePasswordGUI", "ChangePassword", new { EmailAddress = HttpContext.Session.GetString("email") });
                }
                else
                {
                    account.Password = BCrypt.Net.BCrypt.HashPassword(NewPassword);
                    db.Users.Update(account);
                    db.SaveChanges();
                    TempData["success"] = "Mật khẩu đã được cập nhật";
                    return RedirectToAction("ChangePasswordGUI", "ChangePassword", new { EmailAddress = HttpContext.Session.GetString("email") });
                }
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
