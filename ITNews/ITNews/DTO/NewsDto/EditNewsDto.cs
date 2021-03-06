﻿using System.Collections.Generic;

namespace ITNews.DTO.NewsDto
{
    public class EditNewsDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string UserId { get; set; }
        public string MarkDown { get; set; }
        public IEnumerable<CategoryDto> Categories { get; set; }
        public IEnumerable<TagDto> Tags { get; set; }
    }
}
