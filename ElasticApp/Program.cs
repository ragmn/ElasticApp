using ElasticApp.Model;
using Nest;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ElasticApp
{
    class Program
    {
        private static ElasticConnector _elastiClient;

        static void Main(string[] args)
        {
            //Create instance
            CreateElasticClient();
            //create new indexx
            //CreateIndex();
            ////indexing single document
            //insertSingleDoc();
            ////indexing multiple document
            //insertMultipleDoc();
            //fetch documents
            FetchDocument();
        }

        public static void CreateIndex()
        {
            _elastiClient.CreateIndex<Blog>();
        }

        public static void insertMultipleDoc()
        {
            var blog1 = new Blog
            {
                Id = 1,
                Heading = "Heading tow",
                Details = "This has some details",
                Created_Date = DateTime.Now,
                Modified_Date = DateTime.Now,
            };
            var blog2 = new Blog
            {
                Id = 2,
                Heading = "Heading tow",
                Details = "This has some more details",
                Created_Date = DateTime.Now,
                Modified_Date = DateTime.Now,
            };
            var blog3 = new Blog
            {
                Id = 3,
                Heading = "Heading tow",
                Details = "This has some extra details ",
                Created_Date = DateTime.Now,
                Modified_Date = DateTime.Now,
            };
            var lst = new List<Blog>();
            lst.Add(blog1);
            lst.Add(blog2);
            lst.Add(blog3);
            _elastiClient.InsertMany(lst);
        }
        public static void FetchDocument()
        {
            //single doc
            var res = _elastiClient.Client.Get<Blog>(1);
            //query string
            var res1 = _elastiClient.Client.Search<Blog>(s => s
            .Query(q => q
            .QueryString(qs => qs
            .DefaultField(o => o.Details)
            .Query("details")
            )));

        }
        public static void insertSingleDoc()
        {
            var blog = new Blog
            {
                Id = 1,
                Heading = "Heading one",
                Details = "This has some details",
                Created_Date = DateTime.Now,
                Modified_Date = DateTime.Now,
            };
            _elastiClient.InsertSingle(blog);
        }
        public static void CreateElasticClient()
        {
            string indexServerHost = "http://localhost:9200/";
            string indexName = "my_blog";
            try
            {
                _elastiClient = new ElasticConnector(indexServerHost, indexName);
            }
            catch (Exception e)
            {

            }

        }

    }
}
