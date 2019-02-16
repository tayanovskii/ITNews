using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITNews.DTO.UserDto
{
    public class ManageUserDto
    {
        public bool UserBlocked { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CountLikes { get; set; }
        public int CommentLikes { get; set; }
        public IEnumerable<string> Role { get; set; }
    }
}
