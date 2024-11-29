namespace ChapsDotNET.Common;

public class FindContainer
{
    public async Task<string?> FindContainerIPAddressAsync()
    {
        var httpClient = new HttpClient();
        var subnetPrefix = "10.26.49.";

        // Iterate over the last octet from 1 to 254
        for (int i = 1; i < 255; i++)
        {
            var currentIp = $"{subnetPrefix}{i}";
            var url = $"http://{currentIp}:80/";

            try
            {
                // Attempt to connect to the container
                var response = await httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Found container IP address: {currentIp}");
                    return currentIp;
                }
            }
            catch (HttpRequestException)
            {
                // Likely failed because there's no response at that IP, continue to next
                continue;
            }
        }

        // If no IP is found, return null or throw an exception
        Console.WriteLine("Failed to find container IP address within the subnet range.");
        return null;
    }
    
}