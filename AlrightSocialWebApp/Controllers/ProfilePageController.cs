using AlrightSocialWebApp.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AlrightSocialWebApp.Controllers
{
    [Route("ProfilePage")]
    public class ProfilePageController : Controller
    {
        DataContext db = new DataContext();
        private IHostingEnvironment hostingEnvironment;
        public ProfilePageController(IHostingEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;
        }
        [HttpGet]
        public IActionResult Index(string EmailAddress)
        {
            User user = db.GetUserInfo(EmailAddress);
            ViewData.Model = user;
            List<object> postlist = db.GetListOfPost(EmailAddress);
            dynamic mymodel = new ExpandoObject();
            mymodel.User = user;
            mymodel.Posts = postlist;
            return View(mymodel);
        }
        [Route("Information")]
        [HttpGet]
        public IActionResult Information(string EmailAddress)
        {
            User user = db.GetUserInfo(EmailAddress);
            return View(user);
        }
        

        [HttpPost]
        public IActionResult UploadAvatar([FromForm(Name = "avatar")] IFormFile avatar)
        {
            string str = Path.Combine(Directory.GetCurrentDirectory(), hostingEnvironment.WebRootPath, "uploads", HttpContext.Session.GetString("email"));
            if (!Directory.Exists(str))
            {
                Directory.CreateDirectory(str);
            }
            var fileName = "Avatar-" + HttpContext.Session.GetString("email") + avatar.FileName;
            var path = Path.Combine(Directory.GetCurrentDirectory(), hostingEnvironment.WebRootPath, "uploads", HttpContext.Session.GetString("email"), fileName);
            var stream = new FileStream(path, FileMode.Create);
            User user = db.GetUserInfo(HttpContext.Session.GetString("email"));
            user.AvatarURL = "/uploads/" + HttpContext.Session.GetString("email") + "/" + fileName;
            db.Users.Update(user);
            db.SaveChanges();
            avatar.CopyToAsync(stream);
            return RedirectToAction("Index", "ProfilePage", new { EmailAddress = HttpContext.Session.GetString("email") });
        }
    }
}
