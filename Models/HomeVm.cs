using Microsoft.AspNetCore.Http;
using Social_Media.Data.DataTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Social_Media.Models
{
    public class HomeVm
    {
        public ICollection<Post> Posts { get; set; } = new List<Post>();
        public string Description { get; set; }
        public IFormFile Image { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public string CommentDescription { get; set; }
        public IFormFile CommentImage { get; set; }
    }
}
