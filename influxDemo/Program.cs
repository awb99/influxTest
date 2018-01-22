using System;
using System.Linq.Expressions;
using InfluxData.Net;
using InfluxData.Net.InfluxData;
using InfluxData.Net.Common.Enums;
using  InfluxData.Net.InfluxDb;
using InfluxData.Net.InfluxData.Helpers;

namespace influxDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            demo();
        }
        
        static async void demo ()
        {
           try   {
                // InfluxDbClient
               var influxDbClient = new InfluxDbClient("http://metrics.hoertlehner.com:8086/", "flo", "flo", InfluxDbVersion.v_1_0_0); //.v_1_3);
               
               
               Console.WriteLine("Pinging..");
               var responseP = await influxDbClient.Diagnostics.PingAsync();
               Console.WriteLine(responseP);
               
               
            var query = "SELECT * FROM bookingComApi WHERE time > now() - 10h";
            Console.WriteLine("getting series..");
            var response1 = await influxDbClient.Serie.GetSeriesAsync("demo");
            Console.WriteLine(response1);
               Console.WriteLine("getting data..");
               
            var response = await influxDbClient.Client.QueryAsync(query, "demo");
            Console.WriteLine("getting data..done.");
            Console.WriteLine(response);
             

               } catch (Exception ex) {
                   Console.WriteLine("exception " + ex.Message );
              

               }
        }     
    }
}
