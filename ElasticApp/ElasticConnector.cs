using Elasticsearch.Net;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticApp
{
   
    public class ElasticConnector
    {
        public string _indexServer;
        public string _indexName;
        public ElasticClient Client { get; set; }
  
        public ElasticConnector(string indexServer, string indexClient)
        {
            _indexServer = indexServer;
            _indexName = indexClient;
            var nodes = _indexServer.Split(',').Select(s => new Uri(s)).ToList();
            var connectionPool = new StaticConnectionPool(nodes);
            var settings = new ConnectionSettings(connectionPool)
                 .DefaultIndex(_indexName)
                 .MaximumRetries(5)
                 .MaxRetryTimeout(TimeSpan.FromSeconds(20));                 
            Client = new ElasticClient(settings);                 
        }

        public ElasticConnector CreateIndex<T>() where T : class
        {
            if (!Client.IndexExists(_indexName).Exists)
            {
                Client.CreateIndex(_indexName, ci => ci
                    .Settings(s=>s.NumberOfReplicas(1)
                    .NumberOfShards(2))
                    .Mappings(ma => ma
                        .Map<T>(m => m.AutoMap())
                    )
                );
            }
            return this;
        }

        public string InsertSingle<T>(T obj) where T : class
        {
            string error = null;
            CreateIndex<T>();
            var response = Client.Index<T>(obj, i => i
                 .Index(_indexName)
            );
            if (!response.IsValid)
            {
                error = string.Join(" \n", response.ServerError.Error.RootCause.Select(s => s.Reason));
            }
            return error;
        }

        public string InsertMany<T>(List<T> items) where T : class
        {
            CreateIndex<T>();
            var response = Client.IndexMany<T>(items, _indexName);
            return response.Errors.ToString();
        }
    }
}
