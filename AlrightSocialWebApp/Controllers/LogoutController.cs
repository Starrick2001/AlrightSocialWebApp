using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlrightSocialWebApp.Controllers
{
    public class LogoutController : Controller
    {
        [HttpGet]
        [Route("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("email");
            return View("~/Views/HomePage/Index.cshtml");
        }
    }
}
