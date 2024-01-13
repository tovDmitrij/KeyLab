using Minio;
using Minio.DataModel.Args;
using Microsoft.AspNetCore.Http;
using service.v1.configuration.Interfaces;

namespace service.v1.minio
{
    public sealed class MinioService : IMinioService, IDisposable
    {
        private readonly IMinioClient _minio;

        public MinioService(IMinioConfigurationService cfg)
        {
            var endpoint = cfg.GetMinioEndpoint();
            var accessKey = cfg.GetMinioAccessKey();
            var secretKey = cfg.GetMinioSecretKey();

            _minio = new MinioClient()
                .WithEndpoint(endpoint)
                .WithCredentials(accessKey, secretKey)
                .Build();
        }



        public void PushFile(string userID, IFormFile file)
        {
            InitBucket(userID);

            UploadFileIntoBucket(userID, file);
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

        private async void UploadFileIntoBucket(string bucketName, IFormFile file)
        {
            var contentType = file.ContentType;
            var fileType = contentType.Split('/')[1];
            var fileName = $"{Guid.NewGuid()}.{fileType}";
            var fileSize = file.Length;

            var stream = new MemoryStream();
            file.CopyTo(stream);

            var putObjectArgs = new PutObjectArgs()
                .WithBucket(bucketName)
                .WithObject(fileName)
                .WithObjectSize(fileSize)
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