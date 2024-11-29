using System.Net.Http;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Yarp.ReverseProxy.Configuration;


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

            string? ipAddress;

            // Fetch metadata from ECS
            using (var client = new HttpClient())
            {
                var response = await client.GetStringAsync(metadataEndpoint);
                var metadata = JsonDocument.Parse(response);

                ipAddress = metadata.RootElement
                    .GetProperty("Networks")[0]
                    .GetProperty("IPv4Addresses")[0]
                    .GetString();
            }

            if (string.IsNullOrEmpty(ipAddress))
            {
                throw new InvalidOperationException("Could not determine IP address from metadata.");
            }

            return ipAddress;

        }
    }
}