﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITNews.Helpers;

namespace ITNews.Data.Entities
{
    public class UserQuery : IQueryObject
    {
        public bool? UserBlocked { get; set; }
        public string Role { get; set; }
        public string SortBy { get; set; }
        public bool IsSortAscending { get; set; }
        public int Page { get; set; }
        public byte PageSize { get; set; }
    }
}
