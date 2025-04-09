using Azure.Storage.Queues;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace EntraIDGroupAssignTimerFunc
{
    public class QueryAndQueueFunction
    {
        private readonly ILogger<QueryAndQueueFunction> _logger;
        private readonly IConfiguration _config;

        public QueryAndQueueFunction(ILogger<QueryAndQueueFunction> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        [Function("Function1")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("Fetching users to queue in batches.");

            string connectionString = _config["SqlConnectionString"];
            string queueConnectionString = _config["AzureWebJobsStorage"];
            string queueName = "user-batch-processing-queue";

            try
            {
                List<UserData> users = new List<UserData>();

                if (users.Count == 0)
                {
                    _logger.LogInformation("No users found for processing.");
                    return new OkObjectResult("No users found for processing.");
                }

                QueueClient queueClient = new QueueClient(queueConnectionString, queueName);
                await queueClient.CreateIfNotExistsAsync();

                // Process users in batches of 100
                int batchSize = 100;
                for (int i = 0; i < users.Count; i += batchSize)
                {
                    List<UserData> batch = users.GetRange(i, Math.Min(batchSize, users.Count - i));

                    string message = JsonSerializer.Serialize(batch);
                    await queueClient.SendMessageAsync(Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(message)));

                    _logger.LogInformation($"Queued batch {i / batchSize + 1} with {batch.Count} users.");
                }

                return new OkObjectResult("Users queued successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching users from database: {ex.Message}");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
        public class UserData
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
        }
    }
