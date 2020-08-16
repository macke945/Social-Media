using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Social_Media.Data.DataTables
{
    public class ApplicationUser : IdentityUser
    {
        public string Introduction { get; set; }
        public virtual string ProfileImagePath { get; set; }
        public List<Post> Posts { get; set; } = new List<Post>();
    }
}
