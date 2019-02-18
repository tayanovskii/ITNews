using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITNews.Data.Entities;

namespace ITNews.Services.User
{
    public interface IUserService
    {
        Task<QueryResult<ApplicationUser>> GetUsers(UserQuery queryObj);
    }
}
