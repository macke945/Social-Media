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
using Social_Media.Data;
using Microsoft.EntityFrameworkCore;

namespace Social_Media.Controllers
{
    public class HomeController : Controller
    {
        private readonly PostService postService;
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly ProfileService profileService;
        private readonly ApplicationDbContext _context;
        private readonly DislikeService dislikeService;
        private readonly CommentService commentService;

        public HomeController(PostService postService, IHostingEnvironment hostingEnvironment,
            ProfileService profileService, ApplicationDbContext context, DislikeService dislikeService,
            CommentService commentService)
        {
            this.postService = postService;
            this.hostingEnvironment = hostingEnvironment;
            this.profileService = profileService;
            this.dislikeService = dislikeService;
            this.commentService = commentService;
            _context = context;
        }

        public IActionResult Index()
        {
            var vm = new HomeVm();

            var allPosts = postService.GetAllPosts();
            vm.Posts = allPosts;
            var allComments = commentService.GetAllComments();
            vm.Comments = allComments;
            foreach (var posts in allPosts)
            {
                var user = _context.Users.FirstOrDefault(x => x.UserName == posts.UserName);
                posts.ProfileImagePath = user.ProfileImagePath;
            }
            foreach(var comments in allComments)
            {
                var user = _context.Users.FirstOrDefault(x => x.UserName == comments.UserName);
                comments.ProfileImagePath = user.ProfileImagePath;
            }
            return View(vm);
        }
        public IActionResult Contacts()
        {
            return View();
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

                string uniqueFileName = null;
                if (vm.Image == null)
                {
                    postService.AddPost(post);
                }

                else if (postService.IsImage(vm.Image) && vm.Image.Length < (3 * 1024 * 1024))
                {
                    string uploadFolder = Path.Combine(hostingEnvironment.WebRootPath, "post-images");
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


        public async Task<IActionResult> Dislike(HomeVm vm, int id)
        {
            ClaimsPrincipal currentUser = this.User;
            var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            var dislikePost = new DislikePost();

            dislikePost.PostId = id;
            dislikePost.UserId = currentUserId;

            if (dislikeService.UserAbleToDislikePost(dislikePost))
            {
                dislikeService.AddDislikePost(dislikePost);
            }
            else
            {
                dislikeService.RemoveDislikePost(dislikePost);
            }
            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Comment(HomeVm vm, int id)
        {
            var comment = new Comment();

            ClaimsPrincipal currentUser = this.User;
            var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            comment.Description = vm.CommentDescription;
            comment.UserId = currentUserId;
            comment.PostId = id;
            var UserName = commentService.GetUserNameById(currentUserId);
            comment.UserName = UserName;

            string uniqueFileName = null;
            if (vm.CommentImage == null)
            {
                commentService.AddComment(comment);
            }

            else if (commentService.IsImage(vm.CommentImage) && vm.CommentImage.Length < (3 * 1024 * 1024))
            {
                string uploadFolder = Path.Combine(hostingEnvironment.WebRootPath, "comment-images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + vm.CommentImage.FileName;
                string filePath = Path.Combine(uploadFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    vm.CommentImage.CopyTo(fileStream);
                }
                comment.ImagePath = uniqueFileName;
                commentService.AddComment(comment);
            }

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> DislikeComment(HomeVm vm, int id)
        {
            ClaimsPrincipal currentUser = this.User;
            var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            var dislikeComment = new DislikeComment();

            dislikeComment.CommentId = id;
            dislikeComment.UserId = currentUserId;

            if (dislikeService.UserAbleToDislikeComment(dislikeComment))
            {
                dislikeService.AddDislikeComment(dislikeComment);
            }
            else
            {
                dislikeService.RemoveDislikeComment(dislikeComment);
            }
            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
