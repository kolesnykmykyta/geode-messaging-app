using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using BlobStorageAccess.Services.Interfaces;

namespace BlobStorageAccess.Services
{
    public class BlobStorageService : IBlobStorageService
    {
        private readonly BlobServiceClient _serviceClient;
        private readonly BlobContainerClient _containerClient;

        public BlobStorageService(BlobServiceClient serviceClient)
        {
            _serviceClient = serviceClient;
            _containerClient = serviceClient.GetBlobContainerClient("geode");
        }

        public async Task<string> UploadBlobAsync(Stream blob, string blobName)
        {
            BlobClient blobClient = _containerClient.GetBlobClient(blobName);
            await blobClient.UploadAsync(blob, overwrite: true);
            return GetUrlForBlob(blobName);
        }

        private string GetUrlForBlob(string blobName)
        {
            return $"https://{_serviceClient.AccountName}.blob.core.windows.net/{_containerClient.Name}/{blobName}";
        }
    }
}
