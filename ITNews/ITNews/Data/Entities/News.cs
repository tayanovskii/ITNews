using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITNews.Data.Entities
{
    public class News
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public long VisitorCount { get; set; }
        public User User { get; set; }
        public string UserId { get; set; }
        public IEnumerable<NewsCategory> NewsCategories { get; set; }
        public IEnumerable<NewsTag> NewsTags { get; set; }
        public IEnumerable<Comment> Comments { get; set; }
        public IEnumerable<Rating> Ratings { get; set; }


    }
}
