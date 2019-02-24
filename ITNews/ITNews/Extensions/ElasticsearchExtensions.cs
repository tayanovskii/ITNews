using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITNews.Data.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;

namespace ITNews.Extensions
{
    public static class ElasticSearchExtensions
    {
        public static void AddElasticSearch(
            this IServiceCollection services, IConfiguration configuration)
        {
            var url = configuration["ElasticSearch:Url"];
            var defaultIndex = configuration["ElasticSearch:Index"];

            var settings = new ConnectionSettings(new Uri(url))
                .DefaultIndex(defaultIndex);

            AddDefaultMappings(settings);

            var client = new ElasticClient(settings);

            services.AddSingleton<IElasticClient>(client);

            CreateIndex(client, defaultIndex);
        }

        private static void AddDefaultMappings(ConnectionSettings settings)
        {
            settings
                .DefaultMappingFor<News>(m => m
                    .Ignore(n => n.Ratings)
                    .Ignore(n => n.User)
                    .Ignore(n => n.NewsCategories)
                    .Ignore(n => n.NewsTags)
                    .Ignore(n => n.Comments)
                    .Ignore(news => news.MarkDown)
                    .Ignore(news => news.ModifiedBy)
                    .Ignore(news => news.SoftDeleted)
                    .Ignore(news => news.VisitorCount)
                    .PropertyName(n => n.Id, "id")
                );
        }

        private static void CreateIndex(IElasticClient client, string indexName)
        {
            var createIndexResponse = client.CreateIndex(indexName, c => c
                .Mappings(ms => ms
                    .Map<News>(n => n
                        .AutoMap())));

        }
    }
}
