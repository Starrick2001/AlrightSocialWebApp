using AlrightSocialWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlrightSocialWebApp.Controllers
{
    public class ProfilePageController : Controller
    {
        DataContext db = new DataContext();
        public IActionResult Index(string EmailAddress)
        {
            
            User user = db.GetUserInfo(EmailAddress);
            ViewData.Model = user;
            return View();
        }
    }
}
