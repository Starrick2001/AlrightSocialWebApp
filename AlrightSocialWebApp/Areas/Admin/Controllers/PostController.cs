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

        public IActionResult ManageDeletedPost()
        {
            return View(_context.DeletedPost.ToList());
        }

        [HttpPost]
        public IActionResult RestorePost(int PostID)
        {
            int identityPost = _context.identity("Post") + 1;
            int identityNoti = _context.identity("Notification") + 1;
            var deletedPost = _context.DeletedPost.FirstOrDefault(o => o.ID == PostID);
            Post post = new Post
            {
                Author = deletedPost.Author,
                Content = deletedPost.Content,
                ImageURL = deletedPost.ImageURL,
                Privacy = deletedPost.Privacy,
                TimeCreate = deletedPost.TimeCreate,
                TimeModified = deletedPost.TimeModified,
                Title = deletedPost.Title
            };
            _context.Post.Add(post);
            _context.SaveChanges();
            var deletedNotification = _context.DeletedNotification.Where(o => o.PostID == PostID);
            foreach (var item in deletedNotification)
            {
                Notification notification = new Notification
                {
                    UserEmail = item.UserEmail,
                    PostID = identityPost,
                    Time = item.Time,
                    Content = item.Content,
                    IsRead = item.IsRead
                };
                _context.Notification.Add(notification);
            }
            _context.SaveChanges();

            var deletedLike = _context.DeletedLike.Where(o => o.PostID == PostID);
            foreach (var item in deletedLike)
            {
                PostLike postLike = new PostLike
                {
                    PostID = identityPost,
                    NotificationID = (item.NotificationID != null ? identityNoti : item.NotificationID),
                    UserEmail = item.UserEmail
                };
                _context.PostLike.Add(postLike);
                _context.DeletedLike.Remove(item);
            }
            _context.SaveChanges();

            var deletedComment = _context.DeletedComment.Where(o => o.PostID == PostID);
            foreach (var item in deletedComment)
            {
                PostComment postComment = new PostComment
                {
                    UserEmail = item.UserEmail,
                    NotificationID = (item.NotificationID != null ? identityNoti : item.NotificationID),
                    PostID = identityPost,
                    Content = item.Content,
                    Time = item.Time
                };
                _context.PostComment.Add(postComment);
                _context.DeletedComment.Remove(item);
            }
            _context.SaveChanges();

            var deletedShare = _context.DeletedShare.Where(o => o.PostID == PostID);
            foreach (var item in deletedShare)
            {
                PostShare postShare = new PostShare
                {
                    Time = item.Time,
                    PostID = identityPost,
                    NotificationID = (item.NotificationID != null ? identityNoti : item.NotificationID),
                    UserEmail = item.UserEmail,
                    Privacy = item.Privacy
                };
                _context.PostShare.Add(postShare);
                _context.DeletedShare.Remove(item);
            }
            _context.SaveChanges();

            foreach (var item in deletedNotification)
            {
                _context.DeletedNotification.Remove(item);
            }
            _context.SaveChanges();
            _context.DeletedPost.Remove(deletedPost);
            _context.SaveChanges();
            var noti = new Notification()
            {
                Content = "Bài viết của bạn với tiêu đề '" + post.Title + "' đã được quản trị viên khôi phục",
                IsRead = false,
                Time = DateTime.Now,
                UserEmail = post.Author,
                PostID = identityPost
            };
            _context.Notification.Add(noti);
            _context.SaveChanges();
            return RedirectToAction("ManageDeletedPost", "Post");
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
            DeletedPost deletedPost = new DeletedPost
            {
                ID = post.ID,
                Author = post.Author,
                Content = post.Content,
                ImageURL = post.ImageURL,
                Privacy = post.Privacy,
                TimeCreate = post.TimeCreate,
                TimeModified = post.TimeModified,
                Title = post.Title
            };
            _context.DeletedPost.Add(deletedPost);
            _context.SaveChanges();
            var postNotification = _context.Notification.Where(o => o.PostID == id);
            foreach (var item in postNotification)
            {
                DeletedNotification temp = new DeletedNotification
                {
                    ID = item.ID,
                    Content = item.Content,
                    PostID = item.PostID,
                    Time = item.Time,
                    UserEmail = item.UserEmail,
                    IsRead = item.IsRead
                };
                _context.DeletedNotification.Add(temp);
            }
            _context.SaveChanges();
            var postLike = _context.PostLike.Where(o => o.PostID == id);
            foreach (var item in postLike)
            {
                DeletedLike temp = new DeletedLike
                {
                    PostID = item.PostID,
                    NotificationID = item.NotificationID,
                    UserEmail = item.UserEmail
                };
                _context.DeletedLike.Add(temp);
            }
            _context.SaveChanges();
            var postShare = _context.PostShare.Where(o => o.PostID == id);
            foreach (var item in postShare)
            {
                DeletedShare temp = new DeletedShare
                {
                    Id = item.Id,
                    NotificationID = item.NotificationID,
                    PostID = item.PostID,
                    Privacy = item.Privacy,
                    Time = item.Time,
                    UserEmail = item.UserEmail
                };
                _context.DeletedShare.Add(temp);
            }
            _context.SaveChanges();
            var postComment = _context.PostComment.Where(o => o.PostID == id);
            foreach (var item in postComment)
            {
                DeletedComment temp = new DeletedComment
                {
                    ID = item.ID,
                    UserEmail = item.UserEmail,
                    Time = item.Time,
                    PostID = item.PostID,
                    NotificationID = item.NotificationID,
                    Content = item.Content
                };
                _context.DeletedComment.Add(temp);
            }
            _context.SaveChanges();
            _context.DeletePost(id);
            return RedirectToAction("ManagePostPageGUI");
        }

    }
}
