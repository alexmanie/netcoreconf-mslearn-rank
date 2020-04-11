using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Cosmos;
using System.Net;
using System.Collections.Generic;

namespace Netcoreconf.Mslearn
{
    public static class UpdateUser
    {
        [FunctionName("UpdateUser")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = null)] HttpRequest req,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            string name = data?.username;
            string lv = data?.level;
            int p = int.Parse((string)data?.points);
            string lvsp = data?.levelStatusPoints;

            var connectionString = Environment.GetEnvironmentVariable("CosmosDbStorage");
            var cosmosClient = new CosmosClient(connectionString);

            Database database = await cosmosClient.CreateDatabaseIfNotExistsAsync("mslearn");
            Container container = await database.CreateContainerIfNotExistsAsync("users", "/username");

            // Create a new user object
            var userUpdate = new User
            {
                Username = name,
                Level = lv,
                Points = p,
                StatusPoints = lvsp
            };

            try
            {
                QueryDefinition query = new QueryDefinition("SELECT * FROM c WHERE c.username = @username")
                .WithParameter("@username", name);

                List<User> results = new List<User>();
                FeedIterator<User> resultSetIterator = container.GetItemQueryIterator<User>(query);

                while (resultSetIterator.HasMoreResults)
                {
                    FeedResponse<User> response = await resultSetIterator.ReadNextAsync();
                    
                    if (response != null)
                    {
                        results.AddRange(response);
                    }
                }

                if (results.Count.Equals(0))
                {
                    return new BadRequestObjectResult($"Failed to update. User not found.");
                }
                else if (results.Count > 1)
                {
                    return new BadRequestObjectResult($"Failed to update. Too many users with same username.");
                }
                else
                {
                    var u = results[0];
                    userUpdate.Id = u.Id;

                    var userResponse = await container.ReplaceItemAsync(userUpdate, u.Id);
                    return new OkObjectResult(userResponse.Resource.Id);
                }
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                return new BadRequestObjectResult($"Failed to update item. Item not found.");
            }
            catch (CosmosException cosmosException)
            {
                return new BadRequestObjectResult($"Failed to update item. Cosmos Status Code {cosmosException.StatusCode}, Sub Status Code {cosmosException.SubStatusCode}: {cosmosException.Message}.");
            }
        }
    }
}
