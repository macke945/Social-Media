using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Social_Media.Data.DataTables
{
    public class Profile
    {
        public int Id { get; set; }
        public string Introduction { get; set; }
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string ImagePath { get; set; }
    }
}
