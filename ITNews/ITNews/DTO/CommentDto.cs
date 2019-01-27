using System;
using System.Collections.Generic;
using ITNews.Data.Entities;

namespace ITNews.DTO
{
    public class CommentDto
    {
        public int  Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public string UserId { get; set; }
        public string ModifiedBy { get; set; }
        public string UserName { get; set; }
        public int CountLike { get; set; }
    }
}
