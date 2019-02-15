using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ITNews.Data;
using ITNews.Data.Entities;
using ITNews.Helpers;
using Microsoft.EntityFrameworkCore;

namespace ITNews.Services.News
{
    public class NewsService : INewsService
    {
        private readonly ApplicationDbContext context;
        public NewsService(ApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task<QueryResult<Data.Entities.News>> GetNews(NewsQuery queryObj)
        {
            var result = new QueryResult<Data.Entities.News>();

            var query = context.News
                    .Include(news => news.User).ThenInclude(user => user.CommentLikes)
                    .Include(news => news.NewsTags).ThenInclude(tag => tag.Tag)
                    .Include(news => news.NewsCategories).ThenInclude(category => category.Category)
                    .Include(news => news.Comments).ThenInclude(comment => comment.Likes)
                    .Include(news => news.Ratings)
                    .AsQueryable();

            query = query.ApplyFiltering(queryObj);
           

            var columnsMap = new Dictionary<string, Expression<Func<Data.Entities.News, object>>>()
            {
                ["VisitorCount"] = news => news.VisitorCount,
                ["ModifiedAt"] = news => news.ModifiedAt,
                ["Rating"] = news =>  news.Ratings.Average(rating => rating.Value)
            };


            query = query.ApplyOrdering(queryObj, columnsMap);
            
            result.TotalItems = await query.CountAsync();

            query = query.ApplyPaging(queryObj);

            result.Items = await query.ToListAsync();

            return result;
        }
    }
}
