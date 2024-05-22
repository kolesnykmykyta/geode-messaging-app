namespace BlobStorageAccess.Services.Interfaces
{
    public interface IBlobStorageService
    {
        Task UploadBlobAsync(Stream blob, string blobName);
    }
}
