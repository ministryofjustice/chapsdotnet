using System.Text.Json;

namespace ChapsDotNET.Common
{
    public class DynamicProxy
    {
        public async Task<string> GetContainerIpAddressAsync()
        {
            // Get ECS metadata endpoint
            var metadataEndpoint = Environment.GetEnvironmentVariable("ECS_CONTAINER_METADATA_URI_V4");
            Console.WriteLine($"Metadata Endpoint: {metadataEndpoint}");
            string? ipAddress = string.Empty;
            if (!string.IsNullOrEmpty(metadataEndpoint))
            {

                // Fetch metadata from ECS
                try
                {
                    using (var client = new HttpClient())
                    {
                        var response = await client.GetStringAsync(metadataEndpoint);
                        var metadata = JsonDocument.Parse(response);
                        Console.WriteLine($"Metadata: {metadata}");

                        ipAddress = metadata.RootElement
                            .GetProperty("Networks")[0]
                            .GetProperty("IPv4Addresses")[0]
                            .GetString();

                        if (!string.IsNullOrEmpty(ipAddress))
                        {
                            Console.WriteLine($"IP address: {ipAddress}");
                            return ipAddress;
                        }
                    }
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"Failed to retrieve metadata: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Metadata endpoint is not available.");
                throw new InvalidOperationException("Metadata endpoint is not available.");
            }
    
            return ipAddress;
        }
    }
}