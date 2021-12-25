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
    public class PostCommentController : Controller
    {
        private readonly DataContext _context = new DataContext();


        // GET: PostComment
        public async Task<IActionResult> Index()
        {
            return View(await _context.PostComment.ToListAsync());
        }

        // GET: PostComment/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var postComment = await _context.PostComment
                .FirstOrDefaultAsync(m => m.ID == id);
            if (postComment == null)
            {
                return NotFound();
            }

            return View(postComment);
        }

        // GET: PostComment/Create
        public IActionResult Create()
        {
            return View();
        }

        [Route("Delete")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var postComment = await _context.PostComment.FirstOrDefaultAsync(m => m.ID == id);
            int PostID = postComment.PostID;
            var notification = await _context.Notification.FirstOrDefaultAsync(m => m.ID == id);
            _context.PostComment.Remove(postComment);
            await _context.SaveChangesAsync();
            if (notification != null)
            {
                _context.Notification.Remove(notification);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("DetailedPostPage", "Post", new { id = PostID });
        }
        [Route("Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int PostID, string Content)
        {
            if (ModelState.IsValid)
            {
                PostComment cmt = new PostComment()
                {
                    Content = Content,
                    UserEmail = HttpContext.Session.GetString("email"),
                    PostID = PostID
                };
                var post = await _context.Post
                    .FirstOrDefaultAsync(m => m.ID == PostID);
                if (String.Compare(post.Author, cmt.UserEmail, true) == 0)
                    _context.CreateComment(cmt);
                else
                    _context.CreateComment(cmt, post);
            }
            return RedirectToAction("DetailedPostPage", "Post", new { id = PostID });
        }
        // GET: PostComment/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var postComment = await _context.PostComment.FindAsync(id);
            if (postComment == null)
            {
                return NotFound();
            }
            return View(postComment);
        }

        // POST: PostComment/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PostComment postComment)
        {
            int postID = postComment.PostID;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(postComment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostCommentExists(postComment.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return RedirectToAction("DetailedPostPage", "Post", new { id = postID });
        }


        private bool PostCommentExists(int id)
        {
            return _context.PostComment.Any(e => e.ID == id);
        }
    }
}
