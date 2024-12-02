using System.Text.Json;

namespace ChapsDotNET.Common
{
    public class DynamicProxy
    {
        public async Task<string> GetContainerIpAddressAsync()
        {
            // Get ECS metadata endpoint
            var metadataEndpoint = Environment.GetEnvironmentVariable("ECS_CONTAINER_METADATA_URI_V4");
            if (string.IsNullOrEmpty(metadataEndpoint))
            {
                throw new InvalidOperationException("Metadata endpoint is not available.");
            }
           
            if (!metadataEndpoint.StartsWith("http://169.254.170.2/v4/"))
            {
                throw new InvalidOperationException($"Unexpected metadata endpoint format: {metadataEndpoint}");
            }
            
            string? ipAddress = string.Empty;


            Console.WriteLine($"Metadata Endpoint: {metadataEndpoint}");

            // Fetch metadata from ECS
            using (var client = new HttpClient())
            {
                try
                {
                    {
                        Console.WriteLine($"Attempting to fetch metadata from: {metadataEndpoint}");
                        var response = await client.GetStringAsync(metadataEndpoint);
                        Console.WriteLine($"Metadata: {response}");
                        var metadata = JsonDocument.Parse(response);

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