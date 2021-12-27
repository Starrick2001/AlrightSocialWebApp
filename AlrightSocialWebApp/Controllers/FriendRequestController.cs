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
    public class FriendRequestController : Controller
    {
        private readonly DataContext _context = new DataContext();
        public IActionResult ManageFriendRequestGUI()
        {
            List<FriendRequest> list = _context.GetListOfFriendRequests(HttpContext.Session.GetString("email"));
            return View(list);
        }
        [HttpPost, ActionName("InsertFriendRequest")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> InsertFriendRequest(string UserEmail)
        {
            FriendRequest friendRequest = new FriendRequest()
            {
                FriendEmail = HttpContext.Session.GetString("email"),
                UserEmail = UserEmail,
                Time = DateTime.Now
            };
            _context.FriendRequest.Add(friendRequest);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "ProfilePage", new { EmailAddress = UserEmail });
        }

        // POST: FriendRequest/Delete/5
        [HttpPost, ActionName("DeclineFriendRequest")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeclineFriendRequest(string FriendEmail)
        {
            var friendRequest = await _context.FriendRequest.FirstOrDefaultAsync(m => m.UserEmail == HttpContext.Session.GetString("email") && m.FriendEmail == FriendEmail);
            _context.FriendRequest.Remove(friendRequest);
            await _context.SaveChangesAsync();
            return RedirectToAction("ManageFriendRequestGUI");
        }
        [HttpPost, ActionName("AcceptFriendRequest")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AcceptFriendRequest(string FriendEmail)
        {
            var friendRequest = await _context.FriendRequest.FirstOrDefaultAsync(m => m.UserEmail == HttpContext.Session.GetString("email") && m.FriendEmail == FriendEmail);
            _context.FriendRequest.Remove(friendRequest);
            await _context.SaveChangesAsync();
            var friend = new Friend()
            {
                UserEmail = HttpContext.Session.GetString("email"),
                FriendEmail = FriendEmail
            };
            new FriendController().InsertFriend(friend);
            return RedirectToAction("ManageFriendRequestGUI");
        }

    }
}
