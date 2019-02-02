using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITNews.DTO
{
    public class CreateCommentDto
    {
        public string Content { get; set; }
        public string UserId { get; set; }
        public int NewsId { get; set; }
    }
}
