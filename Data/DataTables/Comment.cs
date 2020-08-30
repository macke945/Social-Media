using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Social_Media.Data.DataTables
{
    public class Comment
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public ApplicationUser User { get; set; }
        public string ProfileImagePath { get; set; }
        public DateTime TimeOfPost { get; set; } = DateTime.UtcNow;
        public List<DislikeComment> DislikeComments { get; set; }
        public int PostId { get; set; }
        public Post Post { get; set; }
    }
}
