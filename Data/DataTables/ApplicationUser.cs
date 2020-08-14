using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Social_Media.Data.DataTables
{
    public class ApplicationUser : IdentityUser
    {
        public List<Post> Posts { get; set; } = new List<Post>();
        public Profile Profile { get; set; }
    }
}
