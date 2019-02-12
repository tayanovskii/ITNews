using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ITNews.Data.Entities;
using ITNews.DTO;

namespace ITNews.Mapping
{
    public class QueryResultProfile : Profile
    {
        public QueryResultProfile()
        {
            CreateMap(typeof(QueryResult<>), typeof(QueryResultDto<>));
        }
    }
}
