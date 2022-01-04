using AlrightSocialWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Dynamic;

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
                dynamic mymodel = new ExpandoObject();
                mymodel.Posts = list;
                mymodel.Friends = _context.GetListOfFriends(HttpContext.Session.GetString("email"));
                return View(mymodel);
            }
            else
            {
                List<object> list = _context.GetListOfPublicPost();
                dynamic mymodel = new ExpandoObject();
                mymodel.Posts = list;
                return View(mymodel);
            }
        }
        public IActionResult SuspendedNotification()
        {
            return View();
        }
    }
}
