using service.v1.configuration;

using Minio;
using Minio.DataModel.Args;
using Microsoft.AspNetCore.Http;

namespace service.v1.minio
{
    public sealed class MinioService : IMinioService, IDisposable
    {
        private readonly IMinioClient _minio;

        public MinioService(IConfigurationService cfg)
        {
            var endpoint = cfg.GetMinioEndpoint();
            var port = cfg.GetMinioPort();
            var accessKey = cfg.GetMinioAccessKey();
            var secretKey = cfg.GetMinioSecretKey();

            _minio = new MinioClient()
                .WithEndpoint(endpoint, port)
                .WithCredentials(accessKey, secretKey)
                .Build();
        }



        public void PushFile(string userID, IFormFile file, string fileType)
        {
            InitBucket(userID);

            UploadFileIntoBucket(userID, file, fileType);
        }

        public async Task<byte[]> GetFile(string userID, string fileTitle)
        {
            var fileName = $"{fileTitle}.glb";
            byte[] file = default;

            var getArgs = new GetObjectArgs()
                .WithBucket(userID)
                .WithObject(fileName)
                .WithCallbackStream((stream) =>
                {
                    var memoryStream = new MemoryStream();
                    stream.CopyTo(memoryStream);
                    file = memoryStream.ToArray();
                });

            await _minio.GetObjectAsync(getArgs);

            return file;
        }



        private async void InitBucket(string bucketName)
        {
            var beArgs = new BucketExistsArgs().WithBucket(bucketName);

            if (!await _minio.BucketExistsAsync(beArgs).ConfigureAwait(false))
            {
                var mbArgs = new MakeBucketArgs().WithBucket(bucketName);
                await _minio.MakeBucketAsync(mbArgs).ConfigureAwait(false);
            }
        }

        private async void UploadFileIntoBucket(string bucketName, IFormFile file, string fileType)
        {
            var contentType = $"application/{fileType}";
            var fileName = $"{Guid.NewGuid()}.{fileType}";

            var stream = new MemoryStream();
            file.CopyTo(stream);

            var putObjectArgs = new PutObjectArgs()
                .WithBucket(bucketName)
                .WithObject(fileName)
                .WithStreamData(stream)
                .WithContentType(contentType);

            await _minio.PutObjectAsync(putObjectArgs).ConfigureAwait(false);
        }



        public void Dispose()
        {
            _minio.Dispose();
        }
    }
}