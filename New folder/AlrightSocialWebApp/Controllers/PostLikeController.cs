using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AlrightSocialWebApp.Models;
using Microsoft.AspNetCore.Http;

namespace AlrightSocialWebApp.Controllers
{
    public class PostLikeController : Controller
    {
        private readonly DataContext _context = new DataContext();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> InsertOrDeleteLike(string UserEmail, int PostID)
        {
            if (_context.isSuspended(HttpContext.Session.GetString("email")) == true)
            {
                return RedirectToAction("SuspendedNotification", "HomePage");
            }
            var postLike = _context.PostLike.FirstOrDefault(l => l.UserEmail == HttpContext.Session.GetString("email") && l.PostID == PostID);
            if (postLike != null)
            {
                _context.PostLike.Remove(postLike);
                await _context.SaveChangesAsync();
                var notification = await _context.Notification.FindAsync(postLike.NotificationID);
                if (notification != null)
                {
                    _context.Notification.Remove(notification);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction("DetailedPostPage", "Post", new { id = PostID });
            }
            else
            {
                if (UserEmail != HttpContext.Session.GetString("email"))
                {
                    PostLike like = new PostLike()
                    {
                        UserEmail = HttpContext.Session.GetString("email"),
                        PostID = PostID
                    };
                    _context.InsertLike(like, UserEmail);
                    return RedirectToAction("DetailedPostPage", "Post", new { id = PostID });
                }
                else
                {
                    PostLike like = new PostLike()
                    {
                        UserEmail = HttpContext.Session.GetString("email"),
                        PostID = PostID
                    };
                    _context.InsertLike(like);
                    return RedirectToAction("DetailedPostPage", "Post", new { id = PostID });
                }
            }
        }

    }
}
