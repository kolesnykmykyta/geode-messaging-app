﻿using Application.Dtos;
using Application.Utils.HttpClientWrapper;
using Auth.Dtos;
using Blazored.LocalStorage;
using Geode.Maui.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Geode.Maui.Services
{
    internal class UserService : IUsersService
    {
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
            HttpResponseMessage response = await _httpClient.GetAsync("user/all", queryParams, await _helper.GetAccessTokenAsync());

            if (response.IsSuccessStatusCode)
            {
                return await _helper.DeserializeJsonAsync<IEnumerable<UserInfoDto>>(response);
            }
            else
            {
                return new List<UserInfoDto>();
            }
        }
    }
}
