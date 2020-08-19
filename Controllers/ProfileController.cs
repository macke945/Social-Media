using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Social_Media.Data;
using Social_Media.Data.DataTables;
using Social_Media.Models;
using Social_Media.Services;

namespace Social_Media.Controllers
{
    public class ProfileController : Controller
    {
        private readonly PostService postService;
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly ProfileService profileService;
        private readonly ApplicationDbContext _context;

        public ProfileController(PostService postService, IHostingEnvironment hostingEnvironment, ProfileService profileService, ApplicationDbContext context)
        {
            this.postService = postService;
            this.hostingEnvironment = hostingEnvironment;
            this.profileService = profileService;
            _context = context;
        }
        public IActionResult Index(string id)
        {
            var vm = new ProfileVm();
            ClaimsPrincipal currentUser = this.User;
            var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            var currentUserName = profileService.GetUserNameById(currentUserId);

            
            var imagePath = profileService.GetProfileImagePathByUserId(currentUserId);
            vm.ImagePath = imagePath;
            vm.UserName = id;

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ProfileVm vm)
        {
            ClaimsPrincipal currentUser = this.User;
            var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            string uniqueFileName = null;
            var userName = profileService.GetUserNameById(currentUserId);
            var user = _context.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);
            user.Introduction = vm.Introduction;
            if (vm.Image == null)
            {
                profileService.EditProfile(user);
            }

            else if (profileService.IsImage(vm.Image) && vm.Image.Length < (3 * 1024 * 1024))
            {
                string uploadFolder = Path.Combine(hostingEnvironment.WebRootPath, "profile-images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + vm.Image.FileName;
                string filePath = Path.Combine(uploadFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    vm.Image.CopyTo(fileStream);
                }
                user.ProfileImagePath = uniqueFileName;
                profileService.EditProfile(user);
            }
            return View(vm);
        }
    }
}