using System;
using System.Collections.Generic;
using System.Text;
using Nest;

namespace ElasticApp.Model
{
    [ElasticsearchType(Name = "blogs")]
    public class Blog
    {
        public int Id { get; set; }        
        public string Heading { get; set; }
        public string Details { get; set; }
        public DateTime Created_Date { get; set; }
        public DateTime Modified_Date { get; set; }
    }
}
