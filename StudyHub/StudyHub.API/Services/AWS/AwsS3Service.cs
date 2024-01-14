using Amazon.S3;
using Amazon.S3.Model;
using StudyHub.API.Interfaces.AWS;

namespace StudyHub.API.Services.AWS
{
    public class AwsS3Service : IAwsS3Service
    {
        private readonly IAmazonS3 _amazonS3Client;

        public AwsS3Service(IAmazonS3 amazonS3Client)
        {
            _amazonS3Client = amazonS3Client;
        }

        public async Task<IEnumerable<Models.AWS.S3Object>?> GetFiles(string bucketName, string prefix = "")
        {
            if (!await IsBucketExist(bucketName)) return null;

            var request = new ListObjectsV2Request
            {
                BucketName = bucketName,
                Prefix = prefix
            };
            var response = await _amazonS3Client.ListObjectsV2Async(request);
            return response?.S3Objects?.Select(x => new Models.AWS.S3Object
            {
                Name = x.Key.ToString(),
                PresignedUrl = _amazonS3Client.GetPreSignedURL(new GetPreSignedUrlRequest
                { 
                    BucketName = bucketName,
                    Key = x.Key,
                    Expires = DateTime.UtcNow.AddDays(1)
                })
            });
        }

        public Task<bool> IsBucketExist(string bucketName)
        {
            return Amazon.S3.Util.AmazonS3Util.DoesS3BucketExistV2Async(_amazonS3Client, bucketName);
        }

        public async Task<string?> UploadFile(IFormFile file, string bucketName, string prefix = "")
        {
            if (!await IsBucketExist(bucketName)) return null;

            var key = string.IsNullOrEmpty(prefix) ? file.Name : $"{prefix?.TrimEnd('/')}/{file.Name}";
            var request = new PutObjectRequest
            {
                BucketName = bucketName,
                Key = key,
                InputStream = file.OpenReadStream(),
            };
            request.Metadata.Add("Contnet-Type", file.ContentType);
            await _amazonS3Client.PutObjectAsync(request);
            return key;
        }
    }
}
