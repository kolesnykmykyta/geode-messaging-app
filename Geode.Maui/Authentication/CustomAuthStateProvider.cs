using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Web;
using Application.Utils.HttpClientWrapper;
using Auth.Dtos;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage.Json;

namespace Geode.Maui.Authentication
{
    internal class CustomAuthStateProvider : AuthenticationStateProvider
    {
        const string BearerTokenName = "BearerToken";
        const string AuthApiBase = "https://localhost:7077/api/User";

        private ClaimsPrincipal currentUser = new ClaimsPrincipal(new ClaimsIdentity());
        private readonly ILocalStorageService _localStorage;
        private readonly IHttpClientWrapper _httpClient;

        public CustomAuthStateProvider(ILocalStorageService localStorage, IHttpClientWrapper httpClient)
        {
            _localStorage = localStorage;
            _httpClient = httpClient;
        }

        public override Task<AuthenticationState> GetAuthenticationStateAsync() =>
            Task.FromResult(new AuthenticationState(currentUser));

        public Task LogInAsync(LoginDto dto)
        {
            var loginTask = LogInAsyncCore();
            NotifyAuthenticationStateChanged(loginTask);

            return loginTask;

            async Task<AuthenticationState> LogInAsyncCore()
            {
                var user = await LoginWithExternalProviderAsync(dto);
                currentUser = user;

                return new AuthenticationState(currentUser);
            }
        }

        private async Task<ClaimsPrincipal> LoginWithExternalProviderAsync(LoginDto dto)
        {
            ClaimsIdentity identity = new ClaimsIdentity();
            HttpResponseMessage response = await _httpClient.PostAsync($"{AuthApiBase}/login", dto);

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                await _localStorage.SetItemAsStringAsync(BearerTokenName, responseBody);
                identity = new ClaimsIdentity("Custom");
            }

            var authenticatedUser = new ClaimsPrincipal(identity);

            return await Task.FromResult(authenticatedUser);
        }

        public async Task RegisterAsync(RegisterDto dto)
        {
            HttpResponseMessage response = await _httpClient.PostAsync($"{AuthApiBase}/register", dto);
        }

        public async Task LogoutAsync()
        {
            currentUser = new ClaimsPrincipal(new ClaimsIdentity());
            await _localStorage.RemoveItemAsync(BearerTokenName);
            NotifyAuthenticationStateChanged(
                Task.FromResult(new AuthenticationState(currentUser)));
        }
    }
}
