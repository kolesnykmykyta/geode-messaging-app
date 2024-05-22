using Application.Dtos;
using Application.Utils.HttpClientWrapper;
using Auth.Dtos;
using Blazored.LocalStorage;
using Geode.Maui.Services.Interfaces;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Geode.Maui.Services
{
    internal class UserService : IUsersService
    {
        private const string AllUsersEndpoint = "user/all";
        private const string ProfileEndpoint = "user/profile";
        private const string ProfilePictureEndpoint = "user/profile/picture";

        private readonly IHttpClientWrapper _httpClient;
        private readonly IServicesHelper _helper;

        public UserService(IHttpClientWrapper httpClient, IServicesHelper helper)
        {
            _httpClient = httpClient;
            _helper = helper;
        }

        public async Task<IEnumerable<UserInfoDto>> GetUserListAsync(FilterDto? filter)
        {
            Dictionary<string, string>? queryParams = filter == null ? null : _helper.CreateDictionaryFromObject(filter);
            HttpResponseMessage response = await _httpClient.GetAsync(AllUsersEndpoint, queryParams, await _helper.GetAccessTokenAsync());

            if (response.IsSuccessStatusCode)
            {
                return await _helper.DeserializeJsonAsync<IEnumerable<UserInfoDto>>(response);
            }
            else
            {
                return new List<UserInfoDto>();
            }
        }

        public async Task<UserProfileDto?> GetUserProfileAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync(ProfileEndpoint, null, await _helper.GetAccessTokenAsync());

            if (response.IsSuccessStatusCode)
            {
                return await _helper.DeserializeJsonAsync<UserProfileDto?>(response);
            }
            else
            {
                return default;
            }
        }

        public async Task<bool> UpdateProfilePictureAsync(IBrowserFile picture)
        {
            MultipartFormDataContent content = new MultipartFormDataContent();
            StreamContent pictureContent = new StreamContent(picture.OpenReadStream());
            pictureContent.Headers.ContentType = new MediaTypeHeaderValue(picture.ContentType);
            content.Add(pictureContent, "file", picture.Name);

            HttpResponseMessage response = await _httpClient.PostStreamAsync(ProfilePictureEndpoint, content, await _helper.GetAccessTokenAsync());

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateUserProfileAsync(UserProfileDto dto)
        {
            HttpResponseMessage response = await _httpClient.PutAsync(ProfileEndpoint, dto, await _helper.GetAccessTokenAsync());
            return response.IsSuccessStatusCode;
        }
    }
}
