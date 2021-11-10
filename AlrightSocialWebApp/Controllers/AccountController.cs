using AlrightSocialWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BCrypt.Net;

namespace AlrightSocialWebApp.Controllers
{
    [Route("account")]
    public class AccountController : Controller
    {
        private DataContext db = new DataContext();
        [Route("")]
        [Route("login")]
        [Route("~/")]
        public IActionResult Login()
        {
            return View();
        }
        [HttpGet]
        [Route("signup")]
        public IActionResult SignUp()
        {
            return View("SignUp", new AccountModel());
        }
        [HttpPost]
        [Route("signup")]
        public IActionResult SignUp(AccountModel account)
        {
            account.Password = BCrypt.Net.BCrypt.HashPassword(account.Password);
            db.UserLogin.Add(account);
            db.SaveChanges();
            return RedirectToAction("Login");
        }
    }
}
