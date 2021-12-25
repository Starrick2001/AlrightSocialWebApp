using AlrightSocialWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlrightSocialWebApp.Controllers
{
    public class HomePageController : Controller
    {
        DataContext _context = new DataContext();
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("email") != null)
            {
                List<object> list = _context.GetListOfPostHomePage(HttpContext.Session.GetString("email"));
                return View(list);
            }
            else
            {
                List<object> list = _context.GetListOfPublicPost();
                return View(list);
            }
        }
    }
}
