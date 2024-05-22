using Application.CqrsMessages.Users;
using BlobStorageAccess.Services;
using BlobStorageAccess.Services.Interfaces;
using MediatR;

namespace Application.Handlers.Users
{
    public class ChangeUserPictureCommandHandler : IRequestHandler<ChangeUserPictureCommand>
    {
        private readonly IBlobStorageService _blobStorage;

        public ChangeUserPictureCommandHandler(IBlobStorageService blobStorage)
        {
            _blobStorage = blobStorage;
        }

        public async Task Handle(ChangeUserPictureCommand request, CancellationToken cancellationToken)
        {
            await _blobStorage.UploadBlobAsync(request.PictureStream!, request.UserId);
        }
    }
}
