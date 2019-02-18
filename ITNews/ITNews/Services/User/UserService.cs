using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ITNews.Data;
using ITNews.Data.Entities;
using ITNews.Helpers;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace ITNews.Services.User
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext context;

        public UserService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<QueryResult<ApplicationUser>> GetUsers(UserQuery queryObj)
        {
            var result = new QueryResult<ApplicationUser>();

            var query = context.Users.Include(user => user.Comments).ThenInclude(comment => comment.Likes)
                .AsQueryable();

            query = query.ApplyFiltering(queryObj);


            var columnsMap = new Dictionary<string, Expression<Func<ApplicationUser, object>>>()
            {
                ["CreatedAt"] = user => user.CreatedAt,
                ["CountLikes"] = user => user.Comments.Sum(comment => comment.Likes.Count()),
                ["UserName"] = user => user.UserName
            };

            query = query.ApplyOrdering(queryObj, columnsMap);

            result.TotalItems = await query.CountAsync();

            query = query.ApplyPaging(queryObj);

            result.Items = await query.ToListAsync();

            return result;
        }

        //public int GetUserCountLikes(ApplicationUser user)
        //{
        //    var countLikes = context.CommentLikes.Select(like => like.CommentId).Intersect(user.Comments.Select(comment => comment.Id)).Count();
        //    user.Comments.Where(comment => comment.Id == contextCommentLikes.)
        //    foreach (var applicationUser in users)
        //    {
        //        applicationUser.Comments.
        //    }
        //}
    }
}
