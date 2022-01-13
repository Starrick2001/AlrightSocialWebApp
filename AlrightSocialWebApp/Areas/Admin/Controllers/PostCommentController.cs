using AlrightSocialWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlrightSocialWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PostCommentController : Controller
    {
        private DataContext _context = new DataContext();
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ManageCommentGUI(int PostID)
        {
            return View(_context.PostComment.Where(m => m.PostID == PostID).ToList());
        }
        public async Task<IActionResult> DeletePostCommentForAdmin(int ID)
        {
            var postComment = await _context.PostComment.FirstOrDefaultAsync(m => m.ID == ID);
            int PostID = postComment.PostID;
            string Author = postComment.UserEmail;
            string Content = postComment.Content;
            var notification = await _context.Notification.FirstOrDefaultAsync(m => m.ID == ID);
            _context.PostComment.Remove(postComment);
            await _context.SaveChangesAsync();
            if (notification != null)
            {
                _context.Notification.Remove(notification);
                await _context.SaveChangesAsync();
            }
            var notification1 = new Notification()
            {
                Content = "Bình luận của bạn với nội dung '" + Content + "' đã bị quản trị viên xoá",
                IsRead = false,
                Time = DateTime.Now,
                UserEmail = Author,
                PostID = PostID
            };
            _context.Notification.Add(notification1);
            _context.SaveChanges();
            return RedirectToAction("ManageCommentGUI", "PostComment", new { PostID = PostID });
        }
    }
}
