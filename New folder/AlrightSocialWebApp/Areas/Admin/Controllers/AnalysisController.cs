using AlrightSocialWebApp.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace AlrightSocialWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AnalysisController : Controller
    {
            private DataContext _context = new DataContext();
        public IActionResult AnalysisPageGUI()
        {
            dynamic mymodel = new ExpandoObject();
            mymodel.Users = _context.GetListOfUserForAnalysis();
            mymodel.Posts = _context.GetListOfPostForAnalysis();
            return View(mymodel);
        }
    }
}
