using Application.Dtos;
using Microsoft.AspNetCore.Components.Forms;

namespace Geode.Maui.Services.Interfaces
{
    internal interface IUsersService
    {
        Task<IEnumerable<UserInfoDto>> GetUserListAsync(FilterDto? filter);

        Task<UserProfileDto?> GetUserProfileAsync();

        Task<bool> UpdateUserProfileAsync(UserProfileDto dto);

        Task<ResponseBodyDto> UpdateProfilePictureAsync(IBrowserFile picture);
    }
}
