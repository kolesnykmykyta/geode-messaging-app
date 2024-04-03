using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Auth.Dtos;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage.Json;

namespace Geode.Maui.Authentication
{
    internal class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private ClaimsPrincipal currentUser = new ClaimsPrincipal(new ClaimsIdentity());
        private readonly ILocalStorageService _localStorage;

        const string BearerTokenName = "BearerToken";

        public CustomAuthStateProvider(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
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
            try
            {
                using HttpClient _httpClient = new HttpClient();
                var postBody = JsonSerializer.Serialize(dto);
                StringContent content = new StringContent(postBody, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PostAsync("https://localhost:7077/api/User/login", content);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync(); // Store token here
                    await _localStorage.SetItemAsStringAsync(BearerTokenName, responseBody);
                    Console.Write(responseBody);
                    identity = new ClaimsIdentity("Custom");
                }
            }
            catch
            {
                Console.WriteLine("Failed authorization");
            }

            var authenticatedUser = new ClaimsPrincipal(identity);

            return await Task.FromResult(authenticatedUser);
        }

        public async Task RegisterAsync(RegisterDto dto)
        {
            using HttpClient _httpClient = new HttpClient();
            var postBody = JsonSerializer.Serialize(dto);
            StringContent content = new StringContent(postBody, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PostAsync("https://localhost:7077/api/User/register", content);
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
