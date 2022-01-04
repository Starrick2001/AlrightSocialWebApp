﻿using AlrightSocialWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlrightSocialWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ManageUserController : Controller
    {
        private DataContext _context = new DataContext();
        public IActionResult ManageUserGUI()
        {

            return View(_context.Users.ToList());
        }
        public IActionResult ManageSuspendedUser()
        {
            return View(_context.SuspendedUser.ToList());
        }

        public IActionResult InsertSuspendedUser(string EmailAddress)
        {
            SuspendedUser suspendedUser = new SuspendedUser
            {
                SuspendedEmail = EmailAddress
            };
            _context.SuspendedUser.Add(suspendedUser);
            _context.SaveChanges();
            return RedirectToAction("ManageUserGUI");
        }
        public IActionResult DeleteSuspendedUser(string EmailAddress)
        {
            var user = _context.SuspendedUser.FirstOrDefault(m => m.SuspendedEmail == EmailAddress);
            _context.SuspendedUser.Remove(user);
            _context.SaveChanges();
            return RedirectToAction("ManageSuspendedUser");
        }
    }
}
