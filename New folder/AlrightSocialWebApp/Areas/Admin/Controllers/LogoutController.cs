using AlrightSocialWebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlrightSocialWebApp.Areas.Admin.Controllers
{
    public class LogoutController : Controller
    {

        [Area("Admin")]
        public IActionResult Logout()
        {
            var account = db.Administrator.SingleOrDefault(a => a.EmailAddress.Equals(HttpContext.Session.GetString("email")));
            account.SignInStatus = "Offline";
            db.Administrator.Update(account);
            db.SaveChanges();
            HttpContext.Session.Remove("email");
            HttpContext.Session.Remove("name");
            HttpContext.Session.Remove("AvatarURL");
            return RedirectToAction("Index","Home");
        }
        private DataContext db = new DataContext();
    }
}
