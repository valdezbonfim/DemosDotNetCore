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

            string index = args.Length > 0 ?args[0]: "testedotnetcore01";
            int quantidade = args.Length > 1 ? Convert.ToInt32(args[1]) : 5000000;
            int lote = args.Length > 2 ? Convert.ToInt32(args[2]) : 10000;
            int loteIndex = 1;

            var settings = new ConnectionConfiguration(new Uri("http://elastic:changeme@172.16.1.174:9200"))
                .RequestTimeout(TimeSpan.FromMinutes(2));

            var lowlevelClient = new ElasticLowLevelClient(settings);

            var people = new List<object>();

            for (int i = 1; i <= quantidade; i++)
            {
                Console.Write($"\rProcesando o item: {i}");
                people.Add(new { index = new { _index = index, _type = "person", _id = i.ToString() } });
                people.Add(new { FirstName = $"peaple{i}", LastName = $"name{i}" });

                if(i % lote == 0)
                {
                    Console.WriteLine($"\rEnviando o lote: {loteIndex}. Quantidade de itens: {people.Count}");
                    try
                    {
                        var ndexResponse = lowlevelClient.Bulk<StringResponse>(PostData.MultiJson(people.ToArray()));
                        string responseStream = ndexResponse.Body;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"\rErro ao enviar lote. {ex.Message}");
                    }
                   
                    people.Clear();
                    loteIndex++;
                }
            }
        }
    }   
}
