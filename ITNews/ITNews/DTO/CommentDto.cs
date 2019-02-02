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
        public UserMiniCardDto UserCard { get; set; }
        public int NewsId { get; set; }
        public string ModifiedBy { get; set; }
        public int CountLikes { get; set; }
    }
}
