using Application.CqrsMessages.Users;
using Application.Dtos;
using Azure.Storage.Blobs.Models;
using BlobStorageAccess.Services;
using BlobStorageAccess.Services.Interfaces;
using MediatR;

namespace Application.Handlers.Users
{
    public class ChangeUserPictureCommandHandler : IRequestHandler<ChangeUserPictureCommand, ResponseBodyDto>
    {
        private static List<string> AllowedExtensions = new List<string>(){ ".png", ".jpeg", ".jpg" };

        private readonly IBlobStorageService _blobStorage;

        public ChangeUserPictureCommandHandler(IBlobStorageService blobStorage)
        {
            _blobStorage = blobStorage;
        }

        public async Task<ResponseBodyDto> Handle(ChangeUserPictureCommand request, CancellationToken cancellationToken)
        {
            string fileExtension = Path.GetExtension(request.OriginalName);
            if (!AllowedExtensions.Contains(fileExtension))
            {
                return ResponseBodyDto.FailureResponse("Only images files are allowed");
            }

            string newFileName = $"{request.UserId!}{Path.GetExtension(request.OriginalName)}";
            await _blobStorage.UploadBlobAsync(request.PictureStream!, newFileName);

            return ResponseBodyDto.SuccessResponse();
        }
    }
}
