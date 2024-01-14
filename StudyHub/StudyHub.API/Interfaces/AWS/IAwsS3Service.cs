namespace StudyHub.API.Interfaces.AWS
{
    public interface IAwsS3Service
    {
        Task<bool> IsBucketExist(string bucketName);
        Task<string?> UploadFile(IFormFile file, string bucketName, string prefix = "");
        Task<IEnumerable<Models.AWS.S3Object>?> GetFiles(string bucketName, string prefix = "");
    }
}
