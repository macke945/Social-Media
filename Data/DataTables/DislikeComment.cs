using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Social_Media.Data.DataTables
{
    public class DislikeComment
    {
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public int CommentId { get; set; }
        public Comment Comment { get; set; }
    }
}
