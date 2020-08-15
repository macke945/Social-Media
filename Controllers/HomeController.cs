using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Social_Media.Data.DataTables;
using Social_Media.Models;
using Social_Media.Services;

namespace Social_Media.Controllers
{
    public class HomeController : Controller
    {
        private readonly PostService postService;
        private readonly IHostingEnvironment hostingEnvironment;

        public HomeController(PostService postService, IHostingEnvironment hostingEnvironment)
        {
            this.postService = postService;
            this.hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            var profile = new Profile();
            var vm = new HomeVm();
            vm.ProfileImagePath = profile.ImagePath;
            vm.Posts = postService.GetAllPosts();

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(HomeVm vm)
        {
            if (ModelState.IsValid)
            {
                var profile = new Profile();
                vm.ProfileImagePath = profile.ImagePath;
                ClaimsPrincipal currentUser = this.User;
                var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
                var post = new Post();
                post.Description = vm.Description;
                post.UserId = currentUserId;

                var UserName = postService.GetUserNameById(currentUserId);

                post.UserName = UserName;
                string uniqueFileName = null;
                if (vm.Image == null)
                {
                    postService.AddPost(post);
                }

                else if (postService.IsImage(vm.Image) && vm.Image.Length < (3 * 1024 * 1024))
                {
                    string uploadFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + vm.Image.FileName;
                    string filePath = Path.Combine(uploadFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        vm.Image.CopyTo(fileStream);
                    }
                    post.ImagePath = uniqueFileName;
                    postService.AddPost(post);
                }

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
