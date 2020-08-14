using Social_Media.Data.DataTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Social_Media.Models
{
    public class ProfileVm
    {
        public string Introduction { get; set; }
        public string ImagePath { get; set; }
        public ApplicationUser User { get; set; }
    }
}
