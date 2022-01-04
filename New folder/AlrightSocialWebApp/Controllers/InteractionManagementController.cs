using AlrightSocialWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace AlrightSocialWebApp.Controllers
{
    public class InteractionManagementController : Controller
    {
        DataContext _context = new DataContext();
        public IActionResult InteractionManagementGUI()
        {
            List<object> list = _context.GetListOfFriendsInteraction(HttpContext.Session.GetString("email"));
            return View(list);
        }
    }
}
