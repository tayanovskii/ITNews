using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITNews.Data.Entities
{
    public class NewsCategory
    {
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public int NewsId { get; set; }
        public News News { get; set; }
    }
}
