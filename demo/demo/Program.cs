using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Runtime.InteropServices.ComTypes;
using InfluxData.Net;
using InfluxData.Net.InfluxData;
using InfluxData.Net.Common.Enums;
using  InfluxData.Net.InfluxDb;
using InfluxData.Net.InfluxData.Helpers;
using InfluxData.Net.InfluxDb.Models;
using Microsoft.Win32.SafeHandles;

namespace demo
{
    class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            demo();
            Console.ReadLine();
        }
        
        static async Task<int> demo ()
        {
            try   {
                // InfluxDbClient
                var influxDbClient = new InfluxDbClient("http://metrics.hoertlehner.com:8086/", "flo", "flo", InfluxDbVersion.v_1_0_0); //.v_1_3);
              
                Console.WriteLine("Pinging..");
                var responseP = await influxDbClient.Diagnostics.PingAsync();
                Console.WriteLine("Version: " + responseP.Version);
                Console.WriteLine("ResponseTime: " + responseP.ResponseTime );
               
                Console.WriteLine("getting series..");
                var response1 = await influxDbClient.Serie.GetSeriesAsync("demo");
                Console.WriteLine(response1);
                foreach (var element in response1)
                {
                    Console.WriteLine(element.Name);
                }
                
                Console.WriteLine("getting data..");
                var query = "SELECT * FROM bookingComApi WHERE time > now() - 5m";
                var response = await influxDbClient.Client.QueryAsync(query, "demo");
                foreach (var element in response)
                {
                    Console.WriteLine(element.Name);
                    foreach (var elementColumn in element.Columns)
                    {
                        Console.WriteLine("-COL-" + elementColumn);
                    }
                    foreach (var elementValue in element.Values)
                    {
                        //Console.WriteLine(elementValue);
                        Console.WriteLine("--------------------");
                        foreach (var o in elementValue)
                        {
                            Console.WriteLine(o);
                        }
                    }
                }
                
                Console.WriteLine("getting data..done.");
                Console.WriteLine(response);

                await WriteData(influxDbClient);

            } catch (Exception ex) {
                Console.WriteLine("exception " + ex.Message );
        

            }

            return 13;
        }


        static async Task<bool> WriteData ( InfluxDbClient influxDbClient)
        {
            var response1 = await influxDbClient.Database.CreateDatabaseAsync("stockdemo");
            
            var pointToWrite = new Point()
            {
                Name = "reading", // serie/measurement/table to write into
                Tags = new Dictionary<string, object>()
                {
                    {"SensorId", 8},
                    {"SerialNumber", "00AF123B"}
                },
                Fields = new Dictionary<string, object>()
                {
                    {"SensorState", "act"},
                    {"Humidity", 431},
                    {"Temperature", 22.1},
                    {"Resistance", 34957}
                },
                Timestamp = DateTime.UtcNow // optional (can be set to any DateTime moment)
            };
   

            var response = await influxDbClient.Client.WriteAsync(pointToWrite, "stockdemo");
            return true;
        }
    }
}