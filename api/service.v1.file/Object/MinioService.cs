using Minio;
using Minio.DataModel.Args;
using service.v1.configuration.Interfaces;

namespace service.v1.file.Object
{
    public sealed class MinioService : IObjectFileService
    {
        private readonly IMinioClient _minio;

        public MinioService(IMinioConfigurationService cfg)
        {
            var endpoint = cfg.GetMinioEndpoint();
            var port = cfg.GetMinioPort();
            var accessKey = cfg.GetMinioAccessKey();
            var secretKey = cfg.GetMinioSecretKey();

            _minio = new MinioClient()
                .WithEndpoint("play.min.io")
                //.WithEndpoint("localhost", 6003)
                .WithCredentials(accessKey, secretKey)
                .Build();
        }



        public void AddFile(string bucketName, byte[] file)
        {
            InitBucket(bucketName);

            UploadFileIntoBucket(bucketName, file);
        }

        public async Task<byte[]> GetFile(string bucketName, string fileTitle)
        {
            var fileName = $"{fileTitle}.glb";
            byte[] file = default;

            var getArgs = new GetObjectArgs()
                .WithBucket(bucketName)
                .WithObject(fileName)
                .WithCallbackStream((stream) =>
                {
                    var memoryStream = new MemoryStream();
                    stream.CopyTo(memoryStream);
                    file = memoryStream.ToArray();
                });

            await _minio.GetObjectAsync(getArgs);

            return file!;
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

        private async void UploadFileIntoBucket(string bucketName, byte[] file)
        {
            var fileName = $"{Guid.NewGuid()}.glb";
            var fileSize = file.Length;
            var stream = new MemoryStream(file);
            var contentType = "model/gltf-binary";

            var putObjectArgs = new PutObjectArgs()
                .WithBucket(bucketName)
                .WithObject(fileName)
                .WithObjectSize(fileSize)
                .WithStreamData(stream)
                .WithContentType(contentType);

            await _minio.PutObjectAsync(putObjectArgs).ConfigureAwait(false);
        }
    }
}