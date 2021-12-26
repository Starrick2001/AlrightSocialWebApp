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
    public class UserInformationController : Controller
    {
        private readonly DataContext _context = new DataContext();

        // GET: UserInformation
        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.ToListAsync());
        }

        // GET: UserInformation/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.EmailAddress == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: UserInformation/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: UserInformation/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmailAddress,Password,name,sex,DateOfBirth,PhoneNumber,SignInStatus,AvatarURL")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: UserInformation/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: UserInformation/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(/*[Bind("EmailAddress,Password,name,sex,DateOfBirth,PhoneNumber,SignInStatus,AvatarURL")] */User user)
        {
            var account = checkAccount(user.EmailAddress, user.Password);
            if (account == null)
            {
                TempData["error"] = "Sai địa chỉ Email hoặc mật khẩu";
                return RedirectToAction("Information", "ProfilePage", new { EmailAddress = HttpContext.Session.GetString("email") });
            }
            else
            {
                account.name = user.name;
                account.AvatarURL = user.AvatarURL;
                account.DateOfBirth = user.DateOfBirth;
                account.PhoneNumber = user.PhoneNumber;
                account.sex = user.sex;
                _context.Update(account);
                await _context.SaveChangesAsync();
                TempData["success"] = "Thông tin đã được cập nhật";
                return RedirectToAction("Information", "ProfilePage", new { EmailAddress = HttpContext.Session.GetString("email") });
            }
        }
        private User checkAccount(string Email, string Password)
        {
            var account = _context.Users.SingleOrDefault(a => a.EmailAddress.Equals(Email));
            if (account != null)
            {
                if (BCrypt.Net.BCrypt.Verify(Password, account.Password))
                {
                    return account;
                }
            }
            return null;
        }
        // GET: UserInformation/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.EmailAddress == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: UserInformation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _context.Users.FindAsync(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(string id)
        {
            return _context.Users.Any(e => e.EmailAddress == id);
        }
    }
}
