using System;
using System.Collections.Generic;

namespace ITNews.DTO
{
    public class NewsCardDto
    {
        //todo rating count
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public long VisitorCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public int CommentCount { get; set; }
        public double Rating { get; set; }
    }
}
