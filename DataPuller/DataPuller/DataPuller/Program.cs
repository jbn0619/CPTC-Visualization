using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nest;
using Elasticsearch.Net;
using System.IO;

namespace DataPuller
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = "";
            int timeBeforePulls = 10;
            DateTime lastGrab = DateTime.Now;

            while(true)
            {
                if (DateTime.Now.Second - lastGrab.Second >= timeBeforePulls)
                {
                    var settings = new ConnectionSettings(new Uri("https://elasticsearch4.newscrier.org/")).BasicAuthentication("dataviz", "Jq7stJ&7zL35sHuxV2zp");

                    var client = new ElasticLowLevelClient(settings); //ElasticClient(settings);
                    var res = client.Ping<PingResponse>();
                    Console.WriteLine(res);

                    lastGrab = DateTime.Now;
                    
                }
            }

        }
    }
}
