using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.DataProtection.Repositories;
using System.Xml.Linq;

namespace ChapsDotNET.Common;

public class S3XmlRepository : IXmlRepository
{
    private readonly string _bucketName;
    private readonly string _prefix;
    private readonly IAmazonS3 _s3Client;

    public S3XmlRepository(IAmazonS3 s3Client, string bucketName, string prefix)
    {
        _s3Client = s3Client;
        _bucketName = bucketName;
        _prefix = prefix;
    }

    public IReadOnlyCollection<XElement> GetAllElements()
    {
        var elements = new List<XElement>();
        var request = new ListObjectsV2Request
        {
            BucketName = _bucketName,
            Prefix = _prefix
        };

        var response = _s3Client.ListObjectsV2Async(request).Result;
        foreach (var obj in response.S3Objects)
        {
            var getObjectResponse = _s3Client.GetObjectAsync(_bucketName, obj.Key).Result;
            using var stream = getObjectResponse.ResponseStream;
            var element = XElement.Load(stream);
            elements.Add(element);
        }

        return elements;
    }

    public void StoreElement(XElement element, string friendlyName)
    {
        var key = $"{_prefix}/{friendlyName}.xml";
        using var stream = new MemoryStream();
        element.Save(stream);
        stream.Position = 0;

        var request = new Amazon.S3.Model.PutObjectRequest
        {
            BucketName = _bucketName,
            Key = key,
            InputStream = stream
        };

        _s3Client.PutObjectAsync(request).Wait();
    }
}