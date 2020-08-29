using Social_Media.Data;
using Social_Media.Data.DataTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Social_Media.Services
{
    public class DislikeService
    {
        private readonly ApplicationDbContext context;

        public DislikeService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public void AddDislikePost(DislikePost dislikePost)
        {
            context.Add(dislikePost);
            context.SaveChanges();
        }

        public void RemoveDislikePost(DislikePost dislikePost)
        {
            context.Remove(dislikePost);
            context.SaveChanges();
        }
        public bool UserAbleToDislikePost(DislikePost dislikePost)
        {
            return !context.DislikePost
                .Where(v => v.UserId == dislikePost.UserId && v.PostId == dislikePost.PostId)
                .Any();
        }

    }
}
