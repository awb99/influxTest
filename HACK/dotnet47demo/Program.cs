
using System;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Runtime.InteropServices.ComTypes;
using InfluxData.Net;
using InfluxData.Net.InfluxData;
using InfluxData.Net.Common.Enums;
using  InfluxData.Net.InfluxDb;
using InfluxData.Net.InfluxData.Helpers;
using Microsoft.Win32.SafeHandles;

namespace dotnet47demo
{
    class Program
    {

        /*
         * 
         *      
            <dependentAssembly>
                <assemblyIdentity name="System.Web.Http" publicKeyToken="b03f5f7f11d50a3a" culture="neutral" />
                <bindingRedirect oldVersion="4.0.0.0-4.1.1.1" newVersion="4.1.1.2" />
            </dependentAssembly>
            
            
         * 
         * 
         * */

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
             

            } catch (Exception ex) {
                Console.WriteLine("exception " + ex.Message );
              

            }

            return 13;
        }     
    }
}