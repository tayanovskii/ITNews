using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ITNews.Data;
using ITNews.Data.Entities;
using ITNews.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace ITNews.Services.User
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> userManager;

        public UserService(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public async Task<QueryResult<ApplicationUser>> GetUsersAsync(UserQuery queryObj)
        {
            var result = new QueryResult<ApplicationUser>();

            var query = context.Users.Include(user => user.Comments).ThenInclude(comment => comment.Likes)
                .AsQueryable();

            if (queryObj.Role != null)
            {
                var usersInRoleAsync = await userManager.GetUsersInRoleAsync(queryObj.Role);
                query = usersInRoleAsync.AsQueryable().Include(user => user.Comments).ThenInclude(comment => comment.Likes);
            }

            if (queryObj.UserBlocked.HasValue)
                query = query.Where(user => user.UserBlocked == queryObj.UserBlocked.Value);
          


            var columnsMap = new Dictionary<string, Expression<Func<ApplicationUser, object>>>()
            {
                ["CreatedAt"] = user => user.CreatedAt,
                ["CountLikes"] = user => user.Comments.Sum(comment => comment.Likes.Count()),
                ["UserName"] = user => user.UserName
            };

            query = query.ApplyOrdering(queryObj, columnsMap);

            result.TotalItems = query.Count();

            query = query.ApplyPaging(queryObj);

            result.Items =  query.ToList();

            return result;
        }

    }
}
