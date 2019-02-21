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
                    .PropertyName(n => n.Id, "id")
                )
                .DefaultMappingFor<Comment>(m => m
                    .PropertyName(c => c.Id, "id")
                );
        }

        private static void CreateIndex(IElasticClient client, string indexName)
        {
            var createIndexResponse = client.CreateIndex(indexName, c => c
                .Mappings(ms => ms
                    .Map<News>(n => n
                        .AutoMap<ApplicationUser>()
                        .AutoMap<NewsCategory>()
                        .AutoMap<NewsTag>()
                        .AutoMap<Comment>())));

        }
    }
}
