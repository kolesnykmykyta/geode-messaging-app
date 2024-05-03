using Application.Dtos;
using Auth.Dtos;
using Geode.Api.IntegrationTests.TestHelpers;
using Geode.API;
using System.Net;
using System.Text.Json;
using Xunit;

namespace Geode.Api.IntegrationTests.Controllers
{
    public class MessagesControllerTests : BaseTestClass
    {
        public MessagesControllerTests(CustomWebApplicationFactory<Program> factory) : base(factory) { }

        [Fact]
        public async Task GetAllUserMessages_Unauthorized_ReturnsUnauthorized()
        {
            HttpResponseMessage actual = await _httpClient.GetAsync("/api/messages/all");

            Assert.Equal(HttpStatusCode.Unauthorized, actual.StatusCode);
        }

        [Fact]
        public async Task GetAllUserMessages_Authorized_ReturnsOk()
        {
            await AuthorizeUserAsync();

            HttpResponseMessage actual = await _httpClient.GetAsync("/api/messages/all");

            Assert.Equal(HttpStatusCode.OK, actual.StatusCode);
        }

        [Fact]
        public async Task GetAllUserMessages_Authorized_ReturnsExpectedMessages()
        {
            await AuthorizeUserAsync();
            List<MessageDto> expected = GetAllUserMessages_ExpectedMessages();

            HttpResponseMessage response = await _httpClient.GetAsync("/api/messages/all");
            string responseBody = await response.Content.ReadAsStringAsync();
            List<MessageDto>? actual = JsonSerializer.Deserialize<List<MessageDto>>(responseBody);

            Assert.NotNull(actual);
            Assert.Equal(expected.Count, actual.Count);
            Assert.Equal(expected.OrderBy(x => x.Content), actual.OrderBy(x => x.Content), new MessageDtoEqualityComparer());
        }

        [Fact]
        public async Task GetAllMessagesInChat_Unauthorized_ReturnsUnauthorized()
        {
            HttpResponseMessage actual = await _httpClient.GetAsync("/api/messages/chat/1");

            Assert.Equal(HttpStatusCode.Unauthorized, actual.StatusCode);
        }

        [Fact]
        public async Task GetAllMessagesInChat_NonExistingChat_ReturnsBadRequest()
        {
            await AuthorizeUserAsync();

            HttpResponseMessage actual = await _httpClient.GetAsync("/api/messages/chat/10000");

            Assert.Equal(HttpStatusCode.BadRequest, actual.StatusCode);
        }

        [Fact]
        public async Task GetAllMessagesInChat_ExistingChat_ReturnsOk()
        {
            await AuthorizeUserAsync();

            HttpResponseMessage actual = await _httpClient.GetAsync("/api/messages/chat/10000");

            Assert.Equal(HttpStatusCode.BadRequest, actual.StatusCode);
        }

        [Fact]
        public async Task GetAllMessagesInChat_ExistingChat_ReturnsExpectedMessages()
        {
            await AuthorizeUserAsync();
            List<MessageDto> expected = GetAllMessagesInChat_ExpectedMessages();

            HttpResponseMessage response = await _httpClient.GetAsync("/api/messages/chat/2");
            string responseBody = await response.Content.ReadAsStringAsync();
            List<MessageDto>? actual = JsonSerializer.Deserialize<List<MessageDto>>(responseBody);

            Assert.NotNull(actual);
            Assert.Equal(expected.Count, actual.Count);
            Assert.Equal(expected.OrderBy(x => x.Content), actual.OrderBy(x => x.Content), new MessageDtoEqualityComparer());
        }

        private List<MessageDto> GetAllUserMessages_ExpectedMessages()
        {
            return new List<MessageDto>
            {
                new MessageDto(){ Content = "OwnerMessage1" },
                new MessageDto(){ Content = "OwnerMessage2" },
                new MessageDto(){ Content = "MemberMessage1" },
            };
        }

        private List<MessageDto> GetAllMessagesInChat_ExpectedMessages()
        {
            return new List<MessageDto>
            {
                new MessageDto(){ Content = "MemberMessage1" },
                new MessageDto(){ Content = "MemberMessage2" },
            };
        }
    }
}
