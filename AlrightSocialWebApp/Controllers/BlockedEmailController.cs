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
    public class BlockedEmailController : Controller
    {
        private readonly DataContext _context = new DataContext();
        public IActionResult ManageBlockedEmailGUI()
        {
            var blockedEmails = _context.BlockedEmail.Where(m => m.UserEmail == HttpContext.Session.GetString("email"));
            List<User> list = new List<User>();
            foreach (var item in blockedEmails)
            {
                list.Add(_context.GetUserInfo(item.BlockedUser));
            }
            return View(list);
        }



        // POST: BlockedEmail/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> InsertBlockedEmail(string BlockedUser)
        {
            if (ModelState.IsValid)
            {
                var friend = await _context.Friend.FirstOrDefaultAsync(m => m.UserEmail == HttpContext.Session.GetString("email") && m.FriendEmail == BlockedUser);
                var friend1 = await _context.Friend.FirstOrDefaultAsync(m => m.FriendEmail == HttpContext.Session.GetString("email") && m.UserEmail == BlockedUser);
                if (friend != null)
                    _context.Friend.Remove(friend);
                if (friend1 != null)
                    _context.Friend.Remove(friend1);
                await _context.SaveChangesAsync();

                BlockedEmail blockedEmail = new BlockedEmail()
                {
                    BlockedUser = BlockedUser,
                    UserEmail = HttpContext.Session.GetString("email")
                };
                _context.Add(blockedEmail);
                await _context.SaveChangesAsync();
                return RedirectToAction("ManageBlockedEmailGUI");
            }
            return RedirectToAction("ManageBlockedEmailGUI");
        }

        // POST: BlockedEmail/Delete/5
        [HttpPost, ActionName("DeleteBlockedEmail")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteBlockedEmail(string BlockedUser)
        {
            var blockedEmail = await _context.BlockedEmail.FirstOrDefaultAsync(m => m.UserEmail == HttpContext.Session.GetString("email") && m.BlockedUser == BlockedUser);
            _context.BlockedEmail.Remove(blockedEmail);
            await _context.SaveChangesAsync();
            return RedirectToAction("ManageBlockedEmailGUI");
        }

    }
}
