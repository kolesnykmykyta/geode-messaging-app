using Application.Utils.HttpClientWrapper;
using Auth.Dtos;
using Blazored.LocalStorage;
using Geode.Maui.Services.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Geode.Maui.Authentication
{
    internal class AuthenticationService : IAuthenticationService
    {

        const string BearerTokenName = "BearerToken";
        const string RefreshTokenName = "RefreshToken";

        private readonly ILocalStorageService _localStorage;
        private readonly IHttpClientWrapper _httpClient;
        private readonly IServicesHelper _helper;
        private readonly CustomAuthStateProvider _authStateProvider;

        public AuthenticationService(ILocalStorageService localStorage, IHttpClientWrapper httpClient, IServicesHelper helper, AuthenticationStateProvider stateProvider)
        {
            _localStorage = localStorage;
            _httpClient = httpClient;
            _helper = helper;
            _authStateProvider = (CustomAuthStateProvider)stateProvider;
        }

        public async Task LogInAsync(LoginDto dto)
        {
            ClaimsIdentity identity = new ClaimsIdentity();
            HttpResponseMessage response = await _httpClient.PostAsync("user/login", dto);

            if (response.IsSuccessStatusCode)
            {
                string jsonResponseString = await response.Content.ReadAsStringAsync();
                TokenDto? responseBody = JsonSerializer.Deserialize<TokenDto>(jsonResponseString);

                await _localStorage.SetItemAsStringAsync(BearerTokenName, responseBody!.AccessToken);
                await _localStorage.SetItemAsStringAsync(RefreshTokenName, responseBody!.RefreshToken);
                identity = new ClaimsIdentity("Custom");
            }

            await _authStateProvider.LogInAsync(new ClaimsPrincipal(identity));
        }

        public async Task LogoutAsync()
        {
            await _localStorage.RemoveItemAsync(BearerTokenName);
            await _localStorage.RemoveItemAsync(RefreshTokenName);
            _authStateProvider.Logout();
        }

        public async Task<IEnumerable<string>?> RegisterAsync(RegisterDto dto)
        {
            HttpResponseMessage response = await _httpClient.PostAsync("user/register", dto);
            await _localStorage.SetItemAsStringAsync(await response.Content.ReadAsStringAsync(), "regresponse");

            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                string jsonResponseString = await response.Content.ReadAsStringAsync();
                RegisterResultDto? result = JsonSerializer.Deserialize<RegisterResultDto>(jsonResponseString);
                return result.Errors;
            }

            return null;
        }
    }
}
