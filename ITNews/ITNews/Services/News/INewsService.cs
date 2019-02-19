using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITNews.Data.Entities;

namespace ITNews.Services.News
{
    public interface INewsService
    {
        Task<QueryResult<Data.Entities.News>> GetNewsAsync(NewsQuery queryObj);
    }
}
