using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AlrightSocialWebApp.Models;

namespace AlrightSocialWebApp.Controllers
{
    public class PostShareController : Controller
    {
        private readonly DataContext _context = new DataContext();

        // GET: PostShare
        public async Task<IActionResult> Index()
        {
            return View(await _context.PostShare.ToListAsync());
        }

        // GET: PostShare/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var postShare = await _context.PostShare
                .FirstOrDefaultAsync(m => m.Id == id);
            if (postShare == null)
            {
                return NotFound();
            }

            return View(postShare);
        }

        // GET: PostShare/Create
        [HttpGet]
        public async Task<IActionResult> Create(int PostID)
        {
            var post = await _context.Post.FindAsync(PostID);
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }

        // POST: PostShare/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string Author, [Bind("Id,UserEmail,PostID,Privacy,NotificationID,Time")] PostShare postShare)
        {
            if (Author != postShare.UserEmail)
            {
                postShare.Time = DateTime.Now;
                _context.InsertShare(postShare, Author);
                return RedirectToAction("DetailedPostPage", "Post", new { id = postShare.PostID });
            } else
            {
                postShare.Time = DateTime.Now;
                _context.InsertShare(postShare);
                return RedirectToAction("DetailedPostPage", "Post", new { id = postShare.PostID });
            }
        }

        // GET: PostShare/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var postShare = await _context.PostShare.FindAsync(id);
            if (postShare == null)
            {
                return NotFound();
            }
            return View(postShare);
        }

        // POST: PostShare/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserEmail,PostID,Privacy,NotificationID,Time")] PostShare postShare)
        {
            if (id != postShare.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(postShare);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostShareExists(postShare.Id))
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
            return View(postShare);
        }

        // GET: PostShare/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var postShare = await _context.PostShare
                .FirstOrDefaultAsync(m => m.Id == id);
            if (postShare == null)
            {
                return NotFound();
            }

            return View(postShare);
        }

        // POST: PostShare/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var postShare = await _context.PostShare.FindAsync(id);
            _context.PostShare.Remove(postShare);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostShareExists(int id)
        {
            return _context.PostShare.Any(e => e.Id == id);
        }
    }
}
