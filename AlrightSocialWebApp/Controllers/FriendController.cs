using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AlrightSocialWebApp.Models;
using Microsoft.AspNetCore.Http;
using System.Dynamic;

namespace AlrightSocialWebApp.Controllers
{
    public class FriendController : Controller
    {
        private readonly DataContext _context = new DataContext();

        // GET: Friend
        public async Task<IActionResult> Index()
        {
            return View(await _context.Friend.ToListAsync());
        }

        // GET: Friend/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var friend = await _context.Friend
                .FirstOrDefaultAsync(m => m.UserEmail == id);
            if (friend == null)
            {
                return NotFound();
            }

            return View(friend);
        }

        // GET: Friend/Create
        public IActionResult Create()
        {
            return View();
        }
        public IActionResult ManageFriendGUI()
        {
            List<object> friends = _context.GetListOfFriends(HttpContext.Session.GetString("email"));
            List<object> list = new List<object>();
            foreach (var item in friends)
            {
                list.Add(_context.GetUserInfo(item.GetType().GetProperty("FriendEmail").GetValue(item, null).ToString()));
            }
            dynamic mymodel = new ExpandoObject();
            mymodel.Friends = list;
            mymodel.FriendRequests = _context.GetListOfFriendRequests(HttpContext.Session.GetString("email"));
            mymodel.Chat = friends;
            return View(mymodel);
        }
        public void InsertFriend(Friend friend)
        {
            Friend friend1 = new Friend()
            {
                UserEmail = friend.FriendEmail,
                FriendEmail = friend.UserEmail
            };
            Chat chat = new Chat()
            {
                User1 = friend.UserEmail,
                User2 = friend.FriendEmail
            };
            _context.Add(chat);
            _context.Add(friend);
            _context.Add(friend1);
            _context.SaveChangesAsync();
        }

        // POST: Friend/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserEmail,FriendEmail")] Friend friend)
        {
            if (ModelState.IsValid)
            {
                _context.Add(friend);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(friend);
        }

        // GET: Friend/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var friend = await _context.Friend.FindAsync(id);
            if (friend == null)
            {
                return NotFound();
            }
            return View(friend);
        }

        // POST: Friend/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("UserEmail,FriendEmail")] Friend friend)
        {
            if (id != friend.UserEmail)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(friend);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FriendExists(friend.UserEmail))
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
            return View(friend);
        }

        // GET: Friend/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var friend = await _context.Friend
                .FirstOrDefaultAsync(m => m.UserEmail == id);
            if (friend == null)
            {
                return NotFound();
            }

            return View(friend);
        }

        // POST: Friend/Delete/5
        [HttpPost, ActionName("DeleteFriend")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteFriend(string EmailAddress)
        {
            var friend = await _context.Friend.FirstOrDefaultAsync(m => m.UserEmail == HttpContext.Session.GetString("email") && m.FriendEmail == EmailAddress);
            var friend1 = await _context.Friend.FirstOrDefaultAsync(m => m.FriendEmail == HttpContext.Session.GetString("email") && m.UserEmail == EmailAddress);
            var chat = await _context.Chats.FirstOrDefaultAsync(m => (m.User1 == HttpContext.Session.GetString("email") && m.User2 == EmailAddress) || (m.User2 == HttpContext.Session.GetString("email") && m.User1 == EmailAddress));
            if (chat != null)
            {
                var messages = _context.Message.Where(m => m.ChatId == chat.Id);
                foreach (var item in messages)
                {
                    _context.Message.Remove(item);
                }
                _context.Chats.Remove(chat);
                _context.SaveChanges();
            }
            _context.Friend.Remove(friend);
            _context.Friend.Remove(friend1);
            await _context.SaveChangesAsync();
            return RedirectToAction("ManageFriendGUI");
        }

        [HttpPost("[action]")]
        public IActionResult SearchFriend(string searchString)
        {         
            List<object> friends = _context.SearchFriend(HttpContext.Session.GetString("email"), "%" + searchString + "%");
            List<object> list = new List<object>();
            foreach (var item in friends)
            {
                list.Add(_context.GetUserInfo(item.GetType().GetProperty("FriendEmail").GetValue(item, null).ToString()));
            }
            dynamic mymodel = new ExpandoObject();
            mymodel.Friends = list;
            mymodel.FriendRequests = _context.GetListOfFriendRequests(HttpContext.Session.GetString("email"));
            mymodel.Chat = friends;
            return View("ManageFriendGUI",new { mymodel });
        }

        private bool FriendExists(string id)
        {
            return _context.Friend.Any(e => e.UserEmail == id);
        }
    }
}
