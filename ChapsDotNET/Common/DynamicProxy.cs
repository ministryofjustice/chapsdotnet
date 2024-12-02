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
            
            string ipAddress = "";

            Console.WriteLine($"Metadata Endpoint: {metadataEndpoint}");

            // Fetch metadata from ECS
            using (var client = new HttpClient())
            {
                try
                {
                    Console.WriteLine($"Attempting to fetch metadata from: {metadataEndpoint}");
                    Console.WriteLine($"HttpClient BaseAddress: {client.BaseAddress}");
                    Console.WriteLine($"HttpClient DefaultRequestHeaders: {string.Join(", ", client.DefaultRequestHeaders)}");

                    await Task.Delay(5000); // 5-second delay to allow the container to start up

                    for (int i = 0; i < 3; i++)
                    {
                        try
                        {
                            var response = await client.GetAsync(metadataEndpoint);
                            Console.WriteLine(
                                $"Response Headers: {string.Join(", ", response.Headers.Select(h => $"{h.Key}: {string.Join(", ", h.Value)}"))}");
                            Console.WriteLine($"Response Status Code: {response.StatusCode}");
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
                                throw new InvalidOperationException(
                                    "Failed to retrieve valid IP address from metadata.");
                            }

                            Console.WriteLine($"Extracted IP address: {ipAddress}");
                            return ipAddress;

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Attempt {i + 1}: Failed to fetch metadata: {ex.Message}");
                            
                            await Task.Delay(3000); // Wait 4 seconds before retrying
                        }
                    }
                    ipAddress = "http//:localhost:80:";
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error fetching metadata: {ex.Message}");
                    throw;
                }
            }

            return ipAddress;
        }
    }
}