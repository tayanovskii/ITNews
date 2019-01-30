﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITNews.DTO
{
    public class FullNewsDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public long VisitorCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
        public string MarkDown { get; set; }
        public UserMiniCardDto UserMiniCardDto { get; set; }
        public IEnumerable<TagDto> Tags { get; set; }
        public IEnumerable<CategoryDto> Categories { get; set; }
        public IEnumerable<CommentDto> Comments { get; set; }
        public double Rating { get; set; }
    }
}
