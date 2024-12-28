namespace P2PLearningAPI.Interfaces
{
    public interface IUploadInterface
    {
        public Task<string> UploadFileAsync(IFormFile file);
        public Task<bool> DeleteFileAsync(string filePath);
    }
}
