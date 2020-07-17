using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Social_Media.Data.DataTables;
using Social_Media.Models;
using Social_Media.Services;

namespace Social_Media.Controllers
{
    public class HomeController : Controller
    {
        private readonly PostService postService;

        public HomeController(PostService postService)
        {
            this.postService = postService;
        }

        public IActionResult Index()
        {

            var vm = new HomeVm();

            vm.Posts = postService.GetAllPosts();
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(HomeVm vm)
        {
            if (ModelState.IsValid)
            {
                ClaimsPrincipal currentUser = this.User;
                var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

                var post = new Post();

                post.Description = vm.Description;
                post.UserId = currentUserId;

                var UserName = postService.GetUserNameById(currentUserId);

                post.UserName = UserName;

                postService.AddPost(post);

                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction("Error", "Home", "");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
