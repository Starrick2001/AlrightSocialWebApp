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

        // GET: Post
        public async Task<IActionResult> Index()
        {
            return View(await _context.Post.ToListAsync());
        }

        // GET: Post/Details/5
        public async Task<IActionResult> Details(int? id)
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
            if (ModelState.IsValid)
            {
                post.Title = "asd";
                post.TimeCreate = DateTime.Now;
                post.Author = HttpContext.Session.GetString("email");
                post.Privacy = "Công khai";
                int count = _context.CreatePost(post);
            }
            return View(post);
        }

        // GET: Post/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Title,Content,TimeCreate,TimeModified,Author,Privacy,ImageURL")] Post post)
        {
            if (id != post.ID)
            {
                return NotFound();
            }

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
                return RedirectToAction(nameof(Index));
            }
            return View(post);
        }

        // GET: Post/Delete/5
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
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await _context.Post.FindAsync(id);
            _context.Post.Remove(post);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
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
            var path = Path.Combine(Directory.GetCurrentDirectory(), hostingEnvironment.WebRootPath,"uploads", HttpContext.Session.GetString("email"), fileName);
            var stream = new FileStream(path, FileMode.Create);
            upload.CopyToAsync(stream);
            return new JsonResult(new
            {
                uploaded = 1,
                fileName = upload.FileName,
                url = "uploads/" + HttpContext.Session.GetString("email") + "/" + fileName
            });
        }
    }
}
