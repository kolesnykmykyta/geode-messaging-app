using Application.Utils.HttpClientWrapper;
using Auth.Dtos;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Geode.Maui.Authentication
{
    internal class AuthenticationService : IAuthenticationService
    {

        const string BearerTokenName = "BearerToken";
        const string RefreshTokenName = "RefreshToken";
        const string AuthApiBase = "https://localhost:7077/api/User";

        private readonly ILocalStorageService _localStorage;
        private readonly IHttpClientWrapper _httpClient;
        private readonly CustomAuthStateProvider _authStateProvider;

        public AuthenticationService(ILocalStorageService localStorage, IHttpClientWrapper httpClient, AuthenticationStateProvider stateProvider)
        {
            _localStorage = localStorage;
            _httpClient = httpClient;
            _authStateProvider = (CustomAuthStateProvider)stateProvider;
        }

        public async Task LogInAsync(LoginDto dto)
        {
            ClaimsIdentity identity = new ClaimsIdentity();
            HttpResponseMessage response = await _httpClient.PostAsync($"{AuthApiBase}/login", dto);

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

        public async Task RegisterAsync(RegisterDto dto)
        {
            await _httpClient.PostAsync($"{AuthApiBase}/register", dto);
        }
    }
}
