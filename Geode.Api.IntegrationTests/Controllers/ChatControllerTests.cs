using Application.Dtos;
using Auth.Dtos;
using Azure.Core;
using Geode.API;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using Xunit;

namespace Geode.Api.IntegrationTests.Controllers
{
    public class ChatControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _httpClient;
        private readonly CustomWebApplicationFactory<Program> _factory;

        public ChatControllerTests(CustomWebApplicationFactory<Program> factory)
        {
            _httpClient = factory.CreateClient();
            _factory = factory;
        }

        [Fact]
        public async Task GetUserChats_UnauthorizedUser_ReturnsUnauthorized()
        {
            HttpResponseMessage actual = await _httpClient.GetAsync("/api/chat/all");

            Assert.Equal(HttpStatusCode.Unauthorized, actual.StatusCode);
        }

        [Fact]
        public async Task GetUserChats_AuthorizedUser_ReturnsOk()
        {
            await AuthorizeUserAsync();
            HttpResponseMessage actual = await _httpClient.GetAsync("/api/chat/all");

            Assert.Equal(HttpStatusCode.OK, actual.StatusCode);
        }

        // TODO: Fix collection Assertion
        // [Fact]
        public async Task GetUserChats_AuthorizedUser_ReturnsExpectedChats()
        {
            await AuthorizeUserAsync();
            List<ChatDto> expected = GetUserChats_ExpectedChats();

            HttpResponseMessage response = await _httpClient.GetAsync("/api/chat/all");
            string responseBody = await response.Content.ReadAsStringAsync();
            List<ChatDto>? actual = JsonSerializer.Deserialize<List<ChatDto>>(responseBody);

            Assert.NotNull(actual);
            Assert.Equal(expected.Count, actual.Count);
            foreach (ChatDto dto in expected)
            {
                Assert.Contains(dto, actual);
            }
        }

        [Fact]
        public async Task CreateNewChat_Unauthorized_ReturnsUnauthorized()
        {
            ChatDto newChat = new ChatDto();

            HttpResponseMessage actual = await _httpClient.PostAsJsonAsync("/api/chat", newChat);

            Assert.Equal(HttpStatusCode.Unauthorized, actual.StatusCode);
        }

        [Theory]
        [MemberData(nameof(InvalidChatNames))]
        public async Task CreateNewChat_InvalidChatName_ReturnsBadRequest(string chatName)
        {
            await AuthorizeUserAsync();
            ChatDto newChat = new ChatDto() { Name = chatName };

            HttpResponseMessage actual = await _httpClient.PostAsJsonAsync("/api/chat", newChat);

            Assert.Equal(HttpStatusCode.BadRequest, actual.StatusCode);
        }

        private async Task AuthorizeUserAsync()
        {
            LoginDto loginDto = new LoginDto()
            {
                Email = "test@test.com",
                Password = "Passw0rd_",
            };

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync<LoginDto>("/api/user/login", loginDto);
            string responseBody = await response.Content.ReadAsStringAsync();
            TokenDto? tokens = JsonSerializer.Deserialize<TokenDto>(responseBody);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokens.AccessToken);
        }

        private List<ChatDto> GetUserChats_ExpectedChats()
        {
            return new List<ChatDto>()
            {
                new ChatDto { Id = 1, Name = "Owner", IsUserOwner = true },
                new ChatDto { Id = 2, Name = "Member", IsUserOwner = false}
            };
        }

        public static IEnumerable<object?[]> InvalidChatNames()
        {
            yield return new object?[] { null };
            yield return new object?[] { "sssssssssssssssssssssssssssssss" }; // Too long (31 character)
        }
    }
}
