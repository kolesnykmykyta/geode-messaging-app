using Application.CqrsMessages.Users;
using Application.Dtos;
using Azure.Storage.Blobs.Models;
using BlobStorageAccess.Services;
using BlobStorageAccess.Services.Interfaces;
using DataAccess.Entities;
using DataAccess.UnitOfWork;
using MediatR;

namespace Application.Handlers.Users
{
    public class ChangeUserPictureCommandHandler : IRequestHandler<ChangeUserPictureCommand, ResponseBodyDto>
    {
        private static List<string> AllowedExtensions = new List<string>(){ ".png", ".jpeg", ".jpg" };

        private readonly IBlobStorageService _blobStorage;
        private readonly IUnitOfWork _unitOfWork;

        public ChangeUserPictureCommandHandler(IBlobStorageService blobStorage, IUnitOfWork unitOfWork)
        {
            _blobStorage = blobStorage;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseBodyDto> Handle(ChangeUserPictureCommand request, CancellationToken cancellationToken)
        {
            string fileExtension = Path.GetExtension(request.OriginalName);
            if (!AllowedExtensions.Contains(fileExtension))
            {
                return ResponseBodyDto.FailureResponse("Only images files are allowed");
            }

            string newFileName = $"{request.UserId!}{Path.GetExtension(request.OriginalName)}";
            string newPictureUrl = await _blobStorage.UploadBlobAsync(request.PictureStream!, newFileName);

            if (!string.IsNullOrEmpty(newPictureUrl))
            {
                User userWithUpdatedPicture = new User()
                {
                    Id = request.UserId!,
                    ProfilePictureUrl = newPictureUrl,
                };

                _unitOfWork.GenericRepository<User>().Update(request.UserId!, userWithUpdatedPicture);
                _unitOfWork.SaveChanges();
            }

            return ResponseBodyDto.SuccessResponse();
        }
    }
}
