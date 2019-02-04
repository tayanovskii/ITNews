using System;

namespace ITNews.DTO.UserDto
{
    public class UserDto
    {
        public string UserId { get; set; }
        public int UserProfileId { get; set; }
        public UserProfileDto UserProfile { get; set; }
        public int? LanguageId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int CountLike { get; set; }

    }
}
