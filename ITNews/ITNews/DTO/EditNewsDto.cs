using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITNews.DTO
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
