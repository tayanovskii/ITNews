using System;
using ITNews.DTO.UserDto;

namespace ITNews.DTO.CommentDto
{
    public class CommentDto
    {
        public int  Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public UserMiniCardDto UserCard { get; set; }
        public int NewsId { get; set; }
        public string ModifiedBy { get; set; }
        public int CountLikes { get; set; }
    }
}
