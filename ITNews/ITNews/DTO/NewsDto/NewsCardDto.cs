using System;
using System.Collections.Generic;
using ITNews.DTO.UserDto;

namespace ITNews.DTO.NewsDto
{
    public class NewsCardDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public UserMiniCardDto User { get; set; }
        public IEnumerable<TagDto> Tags { get; set; }
        public IEnumerable<CategoryDto> Categories { get; set; }
        public NewsStatisticDto NewsStatistic { get; set; }
   
    }
}
