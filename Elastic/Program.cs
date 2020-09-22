using System;
using System.Collections.Generic;
using Elasticsearch.Net;
namespace Elastic
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting Connection with ElasticSearch");

            var settings = new ConnectionConfiguration(new Uri("http://elastic:changeme@172.16.1.174:9200"))
                .RequestTimeout(TimeSpan.FromMinutes(2));

            var lowlevelClient = new ElasticLowLevelClient(settings);

            var people = new List<object>();

            for (int i = 1; i < 5000000; i++)
            {
                people.Add(new { index = new { _index = "teste3", _type = "person", _id = i.ToString() } });
                people.Add(new { FirstName = $"peaple{i}", LastName = $"name{i}" });

                if(i % 1000 == 0)
                {
                    var ndexResponse = lowlevelClient.Bulk<StringResponse>(PostData.MultiJson(people.ToArray()));
                    string responseStream = ndexResponse.Body;
                    people.Clear();
                }
            }
        }
    }   
}
