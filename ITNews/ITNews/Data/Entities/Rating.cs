﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITNews.Data.Entities
{
    public class Rating
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public short Value { get; set; }
        public int NewsId { get; set; }
        public News News { get; set; }

    }
}
