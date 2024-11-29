using System.Text.Json;

namespace ChapsDotNET.Common
{
    public class DynamicProxy
    {
        public async Task<string> GetContainerIpAddressAsync()
        {
            // Get ECS metadata endpoint
            var metadataEndpoint = Environment.GetEnvironmentVariable("ECS_CONTAINER_METADATA_URI_V4");
           
            if (!metadataEndpoint.StartsWith("http://169.254.170.2/v4/"))
            {
                throw new InvalidOperationException($"Unexpected metadata endpoint format: {metadataEndpoint}");
            }
            
            string? ipAddress = string.Empty;
            if (string.IsNullOrEmpty(metadataEndpoint))
            {
                if (string.IsNullOrEmpty(metadataEndpoint))
                {
                    throw new InvalidOperationException("Metadata endpoint is not available.");
                }
            }

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

                        ipAddress = metadata.RootElement
                            .GetProperty("Networks")[0]
                            .GetProperty("IPv4Addresses")[0]
                            .GetString();

                        if (string.IsNullOrEmpty(ipAddress))
                        {
                            throw new InvalidOperationException("Metadata endpoint is not available.");

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