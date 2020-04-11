using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Netcoreconf.Mslearn
{
    public static class GetUsers
    {
        [FunctionName("GetUsers")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            var connectionString = Environment.GetEnvironmentVariable("CosmosDbStorage");
            CosmosClient client = new CosmosClient(connectionString);

            var database = client.GetDatabase("mslearn");
            var container = database.GetContainer("users");

            // var sqlQueryText = "SELECT * FROM c";
            var sqlQueryText = "SELECT * FROM c ORDER BY c.points DESC";
            
            QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);
            FeedIterator<User> queryResultSetIterator = container.GetItemQueryIterator<User>(queryDefinition);

            var users = new List<User>();

            while (queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<User> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                foreach (var u in currentResultSet)
                {
                    users.Add(u);
                    Console.WriteLine($"User {u.Username}");
                }
            }

            return new OkObjectResult(users);
        }
    }
}
