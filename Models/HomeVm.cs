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
        public int Id { get; set; }
        public string Description { get; set; }
        public string UserName { get; set; }
        public IFormFile Image { get; set; }
        public DateTime TimeOfPost { get; set; } = DateTime.UtcNow;
    }
}
