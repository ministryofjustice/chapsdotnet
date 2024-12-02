using System.Text.Json;

namespace ChapsDotNET.Common
{
    public class DynamicProxy
    {
        public async Task<string> GetContainerIpAddressAsync()
        {
            // Get ECS metadata endpoint
            var metadataEndpoint = "http://169.254.170.2/v4/a7199647-384e-40b9-a81a-dd17d6f7e2d8"; //Environment.GetEnvironmentVariable("ECS_CONTAINER_METADATA_URI_V4");
            if (string.IsNullOrEmpty(metadataEndpoint))
            {
                throw new InvalidOperationException("Metadata endpoint is not available.");
            }
           
            // if (!metadataEndpoint.StartsWith("http://169.254.170.2/v3") &&
            //     !metadataEndpoint.StartsWith("http://169.254.170.2/v4"))
            // {
            //     throw new InvalidOperationException($"Unexpected metadata endpoint format: {metadataEndpoint}");
            // }
            
            string? ipAddress = string.Empty;

            Console.WriteLine($"Metadata Endpoint: {metadataEndpoint}");

            // Fetch metadata from ECS
            using (var client = new HttpClient())
            {
                try
                {
                    Console.WriteLine($"Attempting to fetch metadata from: {metadataEndpoint}");
                    Console.WriteLine($"HttpClient BaseAddress: {client.BaseAddress}");
                    Console.WriteLine($"HttpClient DefaultRequestHeaders: {string.Join(", ", client.DefaultRequestHeaders)}");

                    var response = await client.GetAsync(metadataEndpoint);
                    
                    Console.WriteLine($"Response Status Code: {response.StatusCode}");
                    Console.WriteLine($"Response Headers: {string.Join(", ", response.Headers.Select(h => $"{h.Key}: {string.Join(", ", h.Value)}"))}");

                    response.EnsureSuccessStatusCode();
                    
                    var content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Response Content: {content}");
                    
                    var metadata = JsonDocument.Parse(content);
                    

                    if (!metadata.RootElement.TryGetProperty("Networks", out var networks))
                    {
                        throw new InvalidOperationException("Metadata does not contain 'Networks'.");
                    }
                    
                    ipAddress = networks[0].GetProperty("IPv4Addresses")[0].GetString();
                    
                    if (string.IsNullOrEmpty(ipAddress))
                    {
                        throw new InvalidOperationException("Failed to retrieve valid IP address from metadata.");
                    }

                    Console.WriteLine($"Extracted IP address: {ipAddress}");
                    return ipAddress;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error fetching metadata: {ex.Message}");
                    throw;
                }
            }
        }
    }
}