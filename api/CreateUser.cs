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

namespace Netcoreconf.Mslearn
{
    public static class CreateUser
    {
        [FunctionName("CreateUser")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
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
            var u = new User
            {
                Id = Guid.NewGuid().ToString(),
                Username = name,
                Level = lv,
                Points = p,
                StatusPoints = lvsp
            };

            try
            {
                /// Read the item to see if it exists.  
                ItemResponse<User> userResponse = await container.ReadItemAsync<User>(u.Username, new PartitionKey(u.Username));
                Console.WriteLine("Item in database with id: {0} already exists\n", userResponse.Resource.Id);

                 return new OkObjectResult(userResponse.Resource.Id);
            }
            catch(CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                // Create an item in the container representing the User. Note we provide the value of the partition key for this item, which is "Username"
                ItemResponse<User> userResponse = await container.CreateItemAsync<User>(u, new PartitionKey(u.Username));

                /// Note that after creating the item, we can access the body of the item with the Resource property off the ItemResponse. 
                /// We can also access the RequestCharge property to see the amount of RUs consumed on this request.
                Console.WriteLine("Created item in database with id: {0} Operation consumed {1} RUs.\n", userResponse.Resource.Id, userResponse.RequestCharge);

                 return new OkObjectResult(userResponse.Resource.Id);
            }
            catch (CosmosException cosmosException)
            {
                return new BadRequestObjectResult($"Failed to create item. Cosmos Status Code {cosmosException.StatusCode}, Sub Status Code {cosmosException.SubStatusCode}: {cosmosException.Message}.");
            }
        }
    }
}
