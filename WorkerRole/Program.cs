using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;

namespace WorkerRole
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            Console.WriteLine("BEGIN");

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(
                "appsettings.json",
                optional: true,
                reloadOnChange: true);
            var configuration = builder.Build();

            var storageAccount =
               CloudStorageAccount.Parse(configuration["StorageConnectionString"]);

            var queueClient = storageAccount.CreateCloudQueueClient();

            var commandsQueue = queueClient.GetQueueReference("telemetryqueue");

            await commandsQueue.CreateIfNotExistsAsync();

            while (true)
            {

                var message = await commandsQueue.GetMessageAsync();
                if (message == null)
                {
                    await Task.Delay(1000);
                    continue;
                }

                Console.WriteLine("Messaggio: " + message);

                var command = JsonConvert.DeserializeObject<JObject>(message.AsString);

                using (var conn = new SqlConnection(configuration["SelfRunningDB"]))
                {
                    conn.Open();

                    var lat = command.Value<double>("Latitude");
                    var lon = command.Value<double>("Longitude");
                    var istante = command.Value<DateTime>("Moment");
                    var idattivita = command.Value<int>("Activity_Id");

                    var insert = await conn.ExecuteAsync("INSERT INTO Telemetry " +
                        "([Latitude],[Longitude],[Moment],[Activity_Id])" +
                        "VALUES (@lat, @lon, @istante, @idattivita)",
                        new { lat, lon, istante, idattivita });

                    conn.Close();

                }
                await commandsQueue.DeleteMessageAsync(message);
            }

        }
    }
}
