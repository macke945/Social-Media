using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Social_Media.Data;
using Social_Media.Data.DataTables;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Social_Media.Services
{
    public class PostService
    {
        private readonly ApplicationDbContext context;

        public PostService(ApplicationDbContext context)
        {
            this.context = context;
        }


        public void AddPost(Post post)
        {
            context.Add(post);
            context.SaveChanges();
        }

        public ICollection<Post> GetAllPosts()
        {
            return context.Post
            .Include(p => p.DislikePosts)
            .OrderByDescending(x => x.TimeOfPost)
                .ToList();
        }

        public string GetUserNameById(string id)
        {
            var user = context.Users
                .Find(id);

            return user.UserName;

        }
        public string GetProfileImagePathByUserId(string id)
        {
            var user = context.Users
                .Find(id);

            return user.ProfileImagePath;

        }
        public bool IsImage(IFormFile formFile)
        {
            if (!string.Equals(formFile.ContentType, "image/jpg", StringComparison.OrdinalIgnoreCase) &&
            !string.Equals(formFile.ContentType, "image/jpeg", StringComparison.OrdinalIgnoreCase) &&
            !string.Equals(formFile.ContentType, "image/pjpeg", StringComparison.OrdinalIgnoreCase) &&
            !string.Equals(formFile.ContentType, "image/gif", StringComparison.OrdinalIgnoreCase) &&
            !string.Equals(formFile.ContentType, "image/x-png", StringComparison.OrdinalIgnoreCase) &&
            !string.Equals(formFile.ContentType, "image/png", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }


            var postedFileExtension = Path.GetExtension(formFile.FileName);
            if (!string.Equals(postedFileExtension, ".jpg", StringComparison.OrdinalIgnoreCase)
                && !string.Equals(postedFileExtension, ".png", StringComparison.OrdinalIgnoreCase)
                && !string.Equals(postedFileExtension, ".gif", StringComparison.OrdinalIgnoreCase)
                && !string.Equals(postedFileExtension, ".jpeg", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            return true;
        }

        public int CountUserDislikes(ApplicationUser user)
        {

            int dislikes = 0;

            foreach (var posts in user.Posts)
                dislikes += posts.DislikePosts.Count();

            return dislikes;
        }
    }
}
