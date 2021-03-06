using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AlrightSocialWebApp.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Dynamic;

namespace AlrightSocialWebApp.Controllers
{
    [Route("post")]
    public class PostController : Controller
    {
        DataContext _context = new DataContext();
        private IHostingEnvironment hostingEnvironment;
        public PostController(IHostingEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;
        }
        [HttpPost("[action]")]
        public IActionResult SearchPublicPost(string searchString)
        {
            List<object> list = _context.SearchPublicPost("%" + searchString + "%");
            return View("SearchPost", list);
        }
        [HttpPost("[action]")]
        public IActionResult SearchPost(string searchString)
        {
            List<object> list = _context.SearchPost(HttpContext.Session.GetString("email"), "%" + searchString + "%");
            return View(list);
        }

        // GET: Post
        [Route("ManagePostPage")]
        [HttpGet]
        public IActionResult ManagePostPage()
        {
            dynamic mymodel = new ExpandoObject();
            mymodel.Posts = _context.GetListOfPost(HttpContext.Session.GetString("email"), HttpContext.Session.GetString("email"));
            mymodel.Friends = _context.GetListOfFriends(HttpContext.Session.GetString("email"));
            return View(mymodel);
        }

        // GET: Post/Details/5
        [Route("DetailedPostPage")]
        [HttpGet]
        public async Task<IActionResult> DetailedPostPage(int id)
        {

            var post = await _context.Post
                .FirstOrDefaultAsync(m => m.ID == id);
            var author = await _context.Users
                .FirstOrDefaultAsync(m => m.EmailAddress == post.Author);
            if (post == null)
            {
                return NotFound();
            }
            dynamic mymodel = new ExpandoObject();
            mymodel.Post = _context.GetPostInformation(id);
            if (HttpContext.Session.GetString("email") != null)
                mymodel.isLiked = _context.isLiked(mymodel.Post.GetType().GetProperty("ID").GetValue(mymodel.Post, null), HttpContext.Session.GetString("email"));
            mymodel.Author = author;
            mymodel.Comment = _context.GetListOfComment(id);
            List<string> postLike = new List<string>();
            foreach (var item in _context.PostLike.Where(x => x.PostID == id))
            {
                postLike.Add(_context.GetUserInfo(item.UserEmail).name);
            }
            mymodel.ListOfLike = postLike.ToList();
            List<string> postCmt = new List<string>();
            foreach (var item in _context.PostComment.Where(x => x.PostID == id))
            {
                postCmt.Add(_context.GetUserInfo(item.UserEmail).name);
            }
            mymodel.ListOfCmt = postCmt.Distinct().ToList();
            List<string> postShare = new List<string>();
            foreach (var item in _context.PostShare.Where(x => x.PostID == id))
            {
                postShare.Add(_context.GetUserInfo(item.UserEmail).name);
            }
            mymodel.ListOfShare = postShare.ToList();
            return View(mymodel);
        }

        // GET: Post/Create
        [Route("create")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Post/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Route("create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Post post)
        {
            if (_context.isSuspended(HttpContext.Session.GetString("email")) == true)
            {
                return RedirectToAction("SuspendedNotification","HomePage");
            }
            if (ModelState.IsValid)
            {
                post.TimeCreate = DateTime.Now;
                post.TimeModified = post.TimeCreate;
                post.Author = HttpContext.Session.GetString("email");
                int count = _context.CreatePost(post);
            }
            return RedirectToAction("ManagePostPage", "Post");
        }

        // GET: Post/Edit/5
        [Route("Edit")]
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (_context.isSuspended(HttpContext.Session.GetString("email")) == true)
            {
                return RedirectToAction("SuspendedNotification", "HomePage");
            }
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Post.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }

        // POST: Post/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Route("edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Post post)
        {
            if (_context.isSuspended(HttpContext.Session.GetString("email")) == true)
            {
                return RedirectToAction("SuspendedNotification", "HomePage");
            }
            post.TimeModified = DateTime.Now;
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(post);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("DetailedPostPage", "Post", new { id = post.ID });
            }
            return RedirectToAction("DetailedPostPage", "Post", new { id = post.ID });
        }

        // GET: Post/Delete/5
        [Route("delete")]
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Post
                .FirstOrDefaultAsync(m => m.ID == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Post/Delete/5
        //[HttpPost, ActionName("Delete")]
        [Route("delete")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            _context.DeletePost(id);
            return RedirectToAction("ManagePostPage", "Post");
        }

        private bool PostExists(int id)
        {
            return _context.Post.Any(e => e.ID == id);
        }

        [Route("UploadCKEditor")]
        [HttpPost]
        public IActionResult UploadCKEditor(IFormFile upload)
        {
            string str = Path.Combine(Directory.GetCurrentDirectory(), hostingEnvironment.WebRootPath, "uploads", HttpContext.Session.GetString("email"));
            if (!Directory.Exists(str))
            {
                Directory.CreateDirectory(str);
            }
            var fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + upload.FileName;
            var path = Path.Combine(Directory.GetCurrentDirectory(), hostingEnvironment.WebRootPath, "uploads", HttpContext.Session.GetString("email"), fileName);
            var stream = new FileStream(path, FileMode.Create);
            upload.CopyToAsync(stream);
            return new JsonResult(new
            {
                uploaded = 1,
                fileName = upload.FileName,
                url = "uploads/" + HttpContext.Session.GetString("email") + "/" + fileName
            });
        }

        [Route("BrowseCKEditor")]
        [HttpGet]
        public IActionResult FileBrowse()
        {
            var dir = new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(), hostingEnvironment.WebRootPath, "uploads", HttpContext.Session.GetString("email")));
            ViewBag.fileInfos = dir.GetFiles();
            return View("FileBrowse");
        }
    }
}
