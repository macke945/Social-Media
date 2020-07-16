using Social_Media.Data;
using Social_Media.Data.DataTables;
using System;
using System.Collections.Generic;
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
                .ToList();
        }

        public string GetUserNameById(string id)
        {
            var user = context.Users
                .Find(id);

            return user.UserName;

        }
    }
}
