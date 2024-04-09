﻿using Application.Dtos;
using Application.Utils.HttpClientWrapper;
using Auth.Dtos;
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

        public UserService(IHttpClientWrapper httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IEnumerable<UserInfoDto>> GetUserListAsync(UserListFilterDto? filter)
        {
            Dictionary<string, string>? queryParams = filter == null ? null : CreateDictionaryFromObject(filter);
            HttpResponseMessage response = await _httpClient.GetAsync("user/all", queryParams);

            if (response.IsSuccessStatusCode)
            {
                string jsonResponseString = await response.Content.ReadAsStringAsync();
                List<UserInfoDto>? responseBody = JsonSerializer.Deserialize<List<UserInfoDto>>(jsonResponseString);
                return responseBody;
            }
            else
            {
                return null;
            }
        }

        private Dictionary<string,string> CreateDictionaryFromObject(object obj)
        {
            Type type = obj.GetType();
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var output = new Dictionary<string, string>();

            foreach (var prop in properties)
            {
                var value = prop.GetValue(obj);
                if (value != null)
                {
                    output.Add(prop.Name, value.ToString());
                }
            }

            return output;
        }
    }
}