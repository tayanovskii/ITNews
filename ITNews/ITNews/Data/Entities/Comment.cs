using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ITNews.Data.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        //public ApplicationUser ModifiedByUser { get; set; }
        public string ModifiedBy { get; set; }
        public int NewsId { get; set; }
        public News News { get; set; }
        public IEnumerable<CommentLike> Likes { get; set; }
    }
}
