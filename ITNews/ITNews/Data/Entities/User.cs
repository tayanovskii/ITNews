using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace ITNews.Data.Entities
{
    public class User : IdentityUser
    {
        public int UserProfileId { get; set; }
        public int LanguageId { get; set; }
        public int RandomRegistrationCode { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }


    }
}
