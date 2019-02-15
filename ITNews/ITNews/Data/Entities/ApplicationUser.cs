using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace ITNews.Data.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public int UserProfileId { get; set; }
        public UserProfile UserProfile { get; set; }
        public int? LanguageId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public bool UserBlocked { get; set; }
        public Language Language { get; set; }
        public IEnumerable<News> News { get; set; }
        public IEnumerable<CommentLike> CommentLikes { get; set; }
        public IEnumerable<Rating> Ratings { get; set; }
        public IEnumerable<Comment> Comments { get; set; }

    }
}
