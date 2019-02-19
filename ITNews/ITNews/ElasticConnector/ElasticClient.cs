using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elasticsearch.Net;
using Nest;

namespace ITNews.ElasticConnector
{
    public class ElasticClient
    {
        //public ElasticClient EsClient()
        //{
        //    var nodes = new Uri[]
        //    {
        //        new Uri("http://localhost:9200")
        //    };

        //    var connectionPool = new StaticConnectionPool(nodes);
        //    var connectionSettings = new ConnectionSettings(connectionPool).DisableDirectStreaming();
        //    var elasticClient = new ElasticClient(connectionSettings);
            
        //    return elasticClient;
        //}
    }
}
