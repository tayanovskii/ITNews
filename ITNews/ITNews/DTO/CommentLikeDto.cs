using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITNews.DTO
{
    public class CommentLikeDto
    {
        public int Id { get; set; }
        public int CommentId { get; set; }
        public string UserId { get; set; }
    }
}
