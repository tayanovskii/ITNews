using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITNews.DTO.UserDto;

namespace ITNews.DTO.NewsDto
{
    public class FindNewsDto : NewsCardDto
    {
        public string Content { get; set; }
    }
}
