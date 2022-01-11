using AlrightSocialWebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlrightSocialWebApp.Areas.Admin.Controllers
{
    public class PostReportController : Controller
    {
        //public IActionResult Index()
        //{
        //    return View();
        //}
        DataContext _context = new DataContext();
        [HttpPost]
        public IActionResult Report(int id, string Content)
        {
            PostReport postReport = new PostReport
            {
                EmailAddress = HttpContext.Session.GetString("email"),
                PostID = id,
                Content = Content,
                Time = DateTime.Now
            };
            _context.PostReport.Add(postReport);
            _context.SaveChanges();
            return RedirectToAction("DetailedPostPage", "Post", new { id = id });
        }
    }
}
