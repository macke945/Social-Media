using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Social_Media.Models;

namespace Social_Media.Controllers
{
    public class ProfileController : Controller
    {
        public IActionResult Index()
        {
            var vm = new ProfileVm();
            return View(vm);
        }
    }
}