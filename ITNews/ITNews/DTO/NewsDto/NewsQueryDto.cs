using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITNews.DTO.NewsDto
{
    public class NewsQueryDto
    {
        public string Category { get; set; }
        public string Tag { get; set; }
        public string UserName { get; set; }
        public string SortBy { get; set; }
        public bool IsSortAscending { get; set; }
        public int Page { get; set; }
        public byte PageSize { get; set; }
    }
}
