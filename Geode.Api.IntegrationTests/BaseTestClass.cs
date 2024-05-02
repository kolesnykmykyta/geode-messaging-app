using Auth.Dtos;
using Geode.API;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text.Json;
using Xunit;

namespace Geode.Api.IntegrationTests
{
    public class BaseTestClass : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        protected readonly HttpClient _httpClient;
        protected readonly CustomWebApplicationFactory<Program> _factory;

        protected const string TestUserName = "test";
        protected const string TestUserEmail = "test@test.com";
        protected const string TestUserPassword = "Passw0rd_";
        protected const string TestUserId = "a5701fde-c6b7-42d8-9f6b-965d7111b39f";

        public BaseTestClass(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _httpClient = factory.CreateClient();
        }

        protected async Task AuthorizeUserAsync()
        {
            LoginDto loginDto = new LoginDto()
            {
                Email = TestUserEmail,
                Password = TestUserPassword,
            };

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync<LoginDto>("/api/user/login", loginDto);
            string responseBody = await response.Content.ReadAsStringAsync();
            TokenDto? tokens = JsonSerializer.Deserialize<TokenDto>(responseBody);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokens.AccessToken);
        }
    }
}
