﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlrightSocialWebApp.Controllers
{
    public class HomePageController : Controller
    {
        public IActionResult Index(int ID)
        {

            return View();
        }
    }
}