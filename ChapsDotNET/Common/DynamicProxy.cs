using System.Net;
using System.Text.Json;

namespace ChapsDotNET.Common
{
    public class DynamicProxy
    {
        private readonly HttpClient _httpClient;        
        
        public DynamicProxy(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetContainerIpAddressAsync()
        {
            // Get ECS metadata endpoint
            var metadataEndpoint = Environment.GetEnvironmentVariable("ECS_CONTAINER_METADATA_URI");
            Console.WriteLine($"ECS_CONTAINER_METADATA_URI: {Environment.GetEnvironmentVariable("ECS_CONTAINER_METADATA_URI")}");
            Console.WriteLine($"ECS_CONTAINER_METADATA_URI_V4: {Environment.GetEnvironmentVariable("ECS_CONTAINER_METADATA_URI_V4")}");
            if (string.IsNullOrEmpty(metadataEndpoint))
            {
                throw new InvalidOperationException("Metadata endpoint is not available.");
            }
            var taskMetadataEndpoint = $"{metadataEndpoint}/task";
            Console.WriteLine($"Task Metadata Endpoint: {taskMetadataEndpoint}");
            
            string ipAddress = "";

            // Fetch metadata from ECS
            try
            {
                Console.WriteLine($"Attempting to fetch metadata from: {taskMetadataEndpoint}");
                Console.WriteLine($"HttpClient BaseAddress: {_httpClient.BaseAddress}");
                Console.WriteLine($"HttpClient DefaultRequestHeaders: {string.Join(", ", _httpClient.DefaultRequestHeaders)}");

                await Task.Delay(5000); 
                await Task.Delay(20000); 

                for (int i = 0; i < 3; i++)
                {
                    try
                    {
                        Console.WriteLine($"Attempting to fetch metadata from: {taskMetadataEndpoint} (Attempt {i + 1})");
                        var response = await _httpClient.GetAsync(taskMetadataEndpoint);
                        Console.WriteLine(
                            $"Response Headers: {string.Join(", ", response.Headers.Select(h => $"{h.Key}: {string.Join(", ", h.Value)}"))}");
                        Console.WriteLine($"Response Status Code: {response.StatusCode}");
                        response.EnsureSuccessStatusCode();

                        var content = await response.Content.ReadAsStringAsync();
                        Console.WriteLine($"Response Content: {content}");

                        var metadata = JsonDocument.Parse(content);
                        Console.WriteLine($"metadata {metadata}");


                        foreach (var container in metadata.RootElement.GetProperty("Containers").EnumerateArray())
                        {
                            var containerName = container.GetProperty("Name").GetString();
                            if (containerName != "chaps-container") continue;
                            ipAddress = container
                                .GetProperty("Networks")[0]
                                .GetProperty("IPv4Addresses")[0]
                                .GetString();

                            if (!string.IsNullOrEmpty(ipAddress))
                            {
                                Console.WriteLine($"Found IP for {containerName} : {ipAddress}");
                                return ipAddress;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Attempt {i + 1}: Failed to fetch metadata: {ex.Message}");
                        await Task.Delay(2000 * (i + 1)); //exponential backoff
                        await Task.Delay(4000 * (i + 1)); //exponential backoff
                    }
                }
                ipAddress = "localhost";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to fetch metadata: {ex.Message}");
                ipAddress = "localhost";
            }

            return ipAddress;
        }
    }
}
