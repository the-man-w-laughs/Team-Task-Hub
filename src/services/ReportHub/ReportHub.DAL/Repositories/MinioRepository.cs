using Minio;
using Microsoft.Extensions.Options;
using Minio.DataModel.Args;
using MinioSettings = ReportHub.DAL.Repositories.Config.MinioSettings;
using ReportHub.DAL.Contracts;

namespace ReportHub.DAL.Repositories;

public class MinioRepository : IMinioRepository
{
    private readonly MinioSettings _minioConfig;
    private readonly IMinioClient _minioClient;

    public MinioRepository(IOptions<MinioSettings> minioConfig, IMinioClient minioClient)
    {
        _minioClient = minioClient;
        _minioConfig = minioConfig.Value;

        var bucketExists = _minioClient
            .BucketExistsAsync(new BucketExistsArgs().WithBucket(_minioConfig.BucketName))
            .GetAwaiter()
            .GetResult();

        if (!bucketExists)
        {
            _minioClient
                .MakeBucketAsync(new MakeBucketArgs().WithBucket(_minioConfig.BucketName))
                .GetAwaiter()
                .GetResult();
        }
    }

    public async Task<string> UploadReportAsync(Stream reportContent)
    {
        string bucketName = _minioConfig.BucketName;
        var contentType = "application/zip";
        Random random = new Random();
        int randomNumber = random.Next(1000, 9999);

        string objectName = $"report_{DateTime.Now:yyyyMMddHHmmss}_{randomNumber}.txt";

        var putArgs = new PutObjectArgs()
            .WithBucket(bucketName)
            .WithObject(objectName)
            .WithStreamData(reportContent)
            .WithObjectSize(reportContent.Length)
            .WithContentType(contentType);

        await _minioClient.PutObjectAsync(putArgs).ConfigureAwait(false);

        return objectName;
    }

    public async Task<Stream> GetFileFromMinioAsync(string objectName)
    {
        string bucketName = _minioConfig.BucketName;
        MemoryStream stream = new MemoryStream();
        var result = await _minioClient.GetObjectAsync(
            new GetObjectArgs()
                .WithBucket(bucketName)
                .WithObject(objectName)
                .WithCallbackStream(inputStream => inputStream.CopyTo(stream))
        );
        stream.Seek(0, SeekOrigin.Begin);
        return stream;
    }

    public async Task<string> DeleteFileFromMinioAsync(string objectName)
    {
        string bucketName = _minioConfig.BucketName;
        await _minioClient.RemoveObjectAsync(
            new RemoveObjectArgs().WithBucket(bucketName).WithObject(objectName)
        );
        return objectName;
    }
}
