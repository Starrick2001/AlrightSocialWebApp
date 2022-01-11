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
            List<object> list = new List<object>();
            foreach (var item in _context.Post.ToList())
            {
                list.Add(new
                {
                    ID = item.ID,
                    Title = item.Title,
                    Content = item.Content,
                    TimeCreate = item.TimeCreate,
                    TimeModified = item.TimeModified,
                    Author = item.Author,
                    AuthorName = _context.GetUserInfo(item.Author).name,
                    Privacy = item.Privacy,
                    AmountOfReport = _context.AmountOfReport(item.ID)
                });
            }
            list = list.OrderByDescending(o => o.GetType().GetProperty("AmountOfReport").GetValue(o, null)).ToList();
            return View(list);
        }
        [HttpPost]
        public IActionResult DeletePostForAdmin(int id)
        {
            var post = _context.Post.FirstOrDefault(m => m.ID == id);
            string Author = post.Author;
            string Title = post.Title;
            var notification = new Notification()
            {
                Content = "Bài viết của bạn với " + _context.AmountOfReport(id) + " lượt báo cáo có tiêu đề '" + Title + "' đã bị quản trị viên xoá",
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
