using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace EntraIDGroupAssignQueuefunc
{
    public class AssignUsersToGroupFunction
    {
        private readonly ILogger<AssignUsersToGroupFunction> _logger;
        private readonly IConfiguration _config;
        private readonly HttpClient _httpClient;

        public AssignUsersToGroupFunction(ILogger<AssignUsersToGroupFunction> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
            _httpClient = new HttpClient();
        }
       
        [FunctionName("AssignUsersToGroupFunction")]
        public async Task Run([QueueTrigger("user-batch-processing-queue", Connection = "AzureWebJobsStorage")] string queueMessage)
        {
            try
            {
                string jsonString = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(queueMessage));
                List<UserData> users = JsonSerializer.Deserialize<List<UserData>>(jsonString);

                if (users == null || users.Count == 0)
                {
                    _logger.LogWarning("No users found in queue message.");
                    return;
                }

                _logger.LogInformation($"Processing batch of {users.Count} users.");

                string groupId = _config["EntraIDGroupId"];  // Get the target group ID from config
                string accessToken = await GetAccessTokenAsync(); // Get Graph API access token

                foreach (var user in users)
                {
                    await AssignUserToGroup(groupId, user, accessToken);
                }

                _logger.LogInformation($"Successfully processed {users.Count} users.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error processing queue message: {ex.Message}");
            }
        }

        private async Task<string> GetAccessTokenAsync()
        {
            var tenantId = _config["TenantId"];
            var clientId = _config["ClientId"];
            var clientSecret = _config["ClientSecret"];

            var tokenUrl = $"https://login.microsoftonline.com/{tenantId}/oauth2/v2.0/token";

            var content = new FormUrlEncodedContent(new[]
            {
            new KeyValuePair<string, string>("grant_type", "client_credentials"),
            new KeyValuePair<string, string>("client_id", clientId),
            new KeyValuePair<string, string>("client_secret", clientSecret),
            new KeyValuePair<string, string>("scope", "https://graph.microsoft.com/.default")
        });

            var response = await _httpClient.PostAsync(tokenUrl, content);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonSerializer.Deserialize<Dictionary<string, string>>(responseString);
            return tokenResponse["access_token"];
        }

        private async Task AssignUserToGroup(string groupId, UserData user, string accessToken)
        {

            string graphUrl = $"https://graph.microsoft.com/v1.0/groups/{groupId}/members/$ref";

            var requestBody = new Dictionary<string, string>
            {
                { "@odata.id", $"https://graph.microsoft.com/v1.0/users/{user.Id}" }
            };

            string jsonPayload = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            using (var request = new HttpRequestMessage(HttpMethod.Post, graphUrl))
            {
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
                request.Content = content;

                HttpResponseMessage response = await _httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Failed to assign user {user.Email} to group: {await response.Content.ReadAsStringAsync()}");
                }
                else
                {
                    _logger.LogInformation($"User {user.Email} assigned to group successfully.");
                }
            }
        }


    }
}
