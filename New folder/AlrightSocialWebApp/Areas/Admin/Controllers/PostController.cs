using AlrightSocialWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlrightSocialWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PostController : Controller
    {
        private DataContext _context = new DataContext();
        public IActionResult ManagePostPageGUI()
        {

            return View(_context.Post.ToList());
        }
        [HttpPost]
        public IActionResult DeletePostForAdmin(int id)
        {
            var post = _context.Post.FirstOrDefault(m => m.ID == id);
            string Author = post.Author;
            string Title = post.Title;
            var notification = new Notification()
            {
                Content = "Bài viết với tiêu đề '" + Title + "' của bạn đã bị quản trị viên xoá",
                IsRead = false,
                Time = DateTime.Now,
                UserEmail = Author,
                PostID = null
            };
            _context.Notification.Add(notification);
            _context.SaveChanges();
            _context.DeletePost(id);
            return RedirectToAction("ManagePostPageGUI");
        }

    }
}
