using System;
using System.Collections.Generic;
using ITNews.DTO.UserDto;

namespace ITNews.DTO.NewsDto
{
    public class FullNewsDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public long VisitorCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public string MarkDown { get; set; }
        public UserMiniCardDto UserMiniCardDto { get; set; }
        public NewsStatisticDto NewsStatistic { get; set; }
        public IEnumerable<TagDto> Tags { get; set; }
        public IEnumerable<CategoryDto> Categories { get; set; }
        public IEnumerable<CommentDto.CommentDto> Comments { get; set; }
        public double Rating { get; set; }
    }
}
