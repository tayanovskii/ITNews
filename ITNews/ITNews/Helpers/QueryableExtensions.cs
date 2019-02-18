using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ITNews.Data.Entities;
using static System.String;

namespace ITNews.Helpers
{
    public static class QueryableExtensions
    {
        public static IQueryable<News> ApplyFiltering(this IQueryable<News> query, NewsQuery queryObj)
        {
            if (queryObj.Tag!=null)
                query = query.Where(news => news.NewsTags.Any(tag => tag.Tag.Name == queryObj.Tag));

            if (queryObj.Category!=null)
                query = query.Where(news => news.NewsCategories.Any(category => category.Category.Name== queryObj.Category));

            if (queryObj.UserName != null)
                query = query.Where(news => news.User.UserName == queryObj.UserName);

            return query;
        }


        public static IQueryable<ApplicationUser> ApplyFiltering(this IQueryable<ApplicationUser> query, UserQuery queryObj)
        {
            if (queryObj.UserBlocked.HasValue)
                query = query.Where(user => user.UserBlocked == queryObj.UserBlocked.Value);

            //if (queryObj.Role != null)
            //    query = query.Where(user => user);


            return query;
        }

        public static IQueryable<T> ApplyOrdering<T>(this IQueryable<T> query, IQueryObject queryObj, Dictionary<string, Expression<Func<T, object>>> columnsMap)
        {
            if (IsNullOrWhiteSpace(queryObj.SortBy) || !columnsMap.ContainsKey(queryObj.SortBy))
                return query;

            if (queryObj.IsSortAscending)
                return query.OrderBy(columnsMap[queryObj.SortBy]);
            else
                return query.OrderByDescending(columnsMap[queryObj.SortBy]);
        }

        public static IQueryable<T> ApplyPaging<T>(this IQueryable<T> query, IQueryObject queryObj)
        {
            if (queryObj.Page <= 0)
                queryObj.Page = 1;

            if (queryObj.PageSize <= 0)
                queryObj.PageSize = 10;

            return query.Skip((queryObj.Page - 1) * queryObj.PageSize).Take(queryObj.PageSize);
        }
    }
}
