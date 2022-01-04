using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlrightSocialWebApp.Models;

namespace AlrightSocialWebApp.Controllers
{
    public class LogoutController : Controller
    {
        DataContext db = new DataContext();
        [HttpGet]
        [Route("logout")]
        public IActionResult Logout()
        {
            var account = db.Users.SingleOrDefault(a => a.EmailAddress.Equals(HttpContext.Session.GetString("email")));
            account.SignInStatus = "Offline";
            db.Users.Update(account);
            db.SaveChanges();
            HttpContext.Session.Remove("email");
            HttpContext.Session.Remove("name");
            HttpContext.Session.Remove("AvatarURL");
            return RedirectToAction("Index", "HomePage");
        }
    }
}
