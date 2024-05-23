namespace BlobStorageAccess.Services.Interfaces
{
    public interface IBlobStorageService
    {
        Task<string> UploadBlobAsync(Stream blob, string blobName);
    }
}
