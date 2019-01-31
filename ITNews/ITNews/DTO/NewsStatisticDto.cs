using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITNews.DTO
{
    public class NewsStatisticDto
    {
        public int CommentCount { get; set; }
        public double Rating { get; set; }
        public long VisitorCount { get; set; }
        public int RatingCount { get; set; }
    }
}
