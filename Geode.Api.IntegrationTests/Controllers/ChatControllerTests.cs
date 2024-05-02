using Application.Dtos;
using Auth.Dtos;
using Azure.Core;
using DataAccess.Entities;
using Geode.Api.IntegrationTests.TestHelpers;
using Geode.API;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using Xunit;

namespace Geode.Api.IntegrationTests.Controllers
{
    public class ChatControllerTests : BaseTestClass
    {
        public ChatControllerTests(CustomWebApplicationFactory<Program> factory) : base(factory)
        {
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

        [Fact]
        public async Task GetUserChats_NoFilter_ReturnsExpectedChats()
        {
            await AuthorizeUserAsync();
            List<ChatDto> expected = GetUserChats_NoFilter_ExpectedChats();

            HttpResponseMessage response = await _httpClient.GetAsync("/api/chat/all");
            string responseBody = await response.Content.ReadAsStringAsync();
            List<ChatDto>? actual = JsonSerializer.Deserialize<List<ChatDto>>(responseBody);

            Assert.NotNull(actual);
            Assert.Equal(expected.Count, actual.Count);
            Assert.Equal(expected.OrderBy(x => x.Id), actual.OrderBy(x => x.Id), new ChatEqualityComparer());
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

        [Theory]
        [MemberData(nameof(InvalidChatNames))]
        public async Task CreateNewChat_InvalidChatName_DoesNothing(string chatName)
        {
            await AuthorizeUserAsync();
            int expected = _factory.DbContext.Chats.Count();
            ChatDto newChat = new ChatDto() { Name = chatName };

            _ = await _httpClient.PostAsJsonAsync("/api/chat", newChat);
            int actual = _factory.DbContext.Chats.Count();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task CreateNewChat_ValidRequest_ReturnsOk()
        {
            await AuthorizeUserAsync();
            ChatDto newChat = new ChatDto() { Name = "test" };

            HttpResponseMessage actual = await _httpClient.PostAsJsonAsync("/api/chat", newChat);

            Assert.Equal(HttpStatusCode.OK, actual.StatusCode);

            ChatCleanup("test");
        }

        [Fact]
        public async Task CreateNewChat_ValidRequest_CreatesNewChat()
        {
            await AuthorizeUserAsync();
            ChatDto newChat = new ChatDto() { Name = "test" };

            _ = await _httpClient.PostAsJsonAsync("/api/chat", newChat);

            Assert.True(_factory.DbContext.Chats.Any(x => x.Name == "test"));

            ChatCleanup("test");
        }

        [Fact]
        public async Task CreateNewChat_ValidRequest_AddsOwnerToChat()
        {
            await AuthorizeUserAsync();
            ChatDto newChat = new ChatDto() { Name = "test" };
            string ownerId = _factory.DbContext.Users
                .First(x => x.UserName == "test")
                .Id;

            _ = await _httpClient.PostAsJsonAsync("/api/chat", newChat);
            int newChatId = _factory.DbContext.Chats.First(x => x.Name == "test").Id;

            Assert.True(_factory.DbContext.ChatMembers.Any(x => x.ChatId == newChatId && x.UserId == ownerId));

            ChatCleanup("test");
        }

        [Fact]
        public async Task UpdateChat_Unauthorized_ReturnsUnauthorized()
        {
            ChatDto updatedChat = new ChatDto();

            HttpResponseMessage actual = await _httpClient.PutAsJsonAsync("/api/chat", updatedChat);

            Assert.Equal(HttpStatusCode.Unauthorized, actual.StatusCode);
        }

        [Fact]
        public async Task UpdateChat_UserNotChatOwner_ReturnsBadRequest()
        {
            await AuthorizeUserAsync();
            ChatDto updatedChat = new ChatDto() { Id = 2, Name = "NewChat" };

            HttpResponseMessage actual = await _httpClient.PutAsJsonAsync("/api/chat", updatedChat);

            Assert.Equal(HttpStatusCode.BadRequest, actual.StatusCode);
        }

        [Fact]
        public async Task UpdateChat_UserNotChatOwner_DoesntUpdateChat()
        {
            await AuthorizeUserAsync();
            string oldName = _factory.DbContext.Chats
                .AsNoTracking()
                .First(x => x.Id == 2)
                .Name;
            ChatDto updatedChat = new ChatDto() { Id = 2, Name = "NewChat" };

            _ = await _httpClient.PutAsJsonAsync("/api/chat", updatedChat);

            string newName = _factory.DbContext.Chats
                .AsNoTracking()
                .First(x => x.Id == 2)
                .Name;

            Assert.Equal(oldName, newName);
        }

        [Fact]
        public async Task UpdateChat_ValidRequest_ReturnsOk()
        {
            await AuthorizeUserAsync();
            ChatDto updatedChat = new ChatDto() { Id = 1, Name = "Owner" };

            HttpResponseMessage actual = await _httpClient.PutAsJsonAsync("/api/chat", updatedChat);

            Assert.Equal(HttpStatusCode.OK, actual.StatusCode);
        }

        [Fact]
        public async Task UpdateChat_ValidRequest_ChangesChatName()
        {
            await AuthorizeUserAsync();
            ChatDto updatedChat = new ChatDto() { Id = 1, Name = "NewName" };
            Chat oldChat = _factory.DbContext.Chats
                .AsNoTracking()
                .First(x => x.Id == 1);
            string oldName = oldChat.Name;

            _ = await _httpClient.PutAsJsonAsync("/api/chat", updatedChat);

            string newName = _factory.DbContext.Chats
                .AsNoTracking()
                .First(x => x.Id == 1)
                .Name;
            Assert.NotEqual(oldName, newName);
            Assert.Equal(updatedChat.Name, newName);

            _factory.DbContext.Chats.Update(oldChat);
            _factory.DbContext.SaveChanges();
        }

        [Fact]
        public async Task DeleteChat_Unauthorized_ReturnUnauthorized()
        {
            HttpResponseMessage actual = await _httpClient.DeleteAsync("/api/chat/delete/1");

            Assert.Equal(HttpStatusCode.Unauthorized, actual.StatusCode);
        }

        [Fact]
        public async Task DeleteChat_UserNotOwner_ReturnsBadRequest()
        {
            await AuthorizeUserAsync();

            HttpResponseMessage actual = await _httpClient.DeleteAsync("/api/chat/delete/2");

            Assert.Equal(HttpStatusCode.BadRequest, actual.StatusCode);
        }

        [Fact]
        public async Task DeleteChat_UserNotOwner_DoesntDeleteChat()
        {
            await AuthorizeUserAsync();

            _ = await _httpClient.DeleteAsync("/api/chat/delete/2");

            Assert.True(_factory.DbContext.Chats.Any(x => x.Id == 2));
        }

        [Fact]
        public async Task DeleteChat_NonExistingChat_ReturnsBadRequest()
        {
            await AuthorizeUserAsync();

            HttpResponseMessage actual = await _httpClient.DeleteAsync("/api/chat/delete/10000");

            Assert.Equal(HttpStatusCode.BadRequest, actual.StatusCode);
        }

        [Fact]
        public async Task DeleteChat_ValidRequest_ReturnsOk()
        {
            await AuthorizeUserAsync();
            int chatToDeleteId = CreateTestChat();

            HttpResponseMessage actual = await _httpClient.DeleteAsync($"/api/chat/delete/{chatToDeleteId}");

            Assert.Equal(HttpStatusCode.OK, actual.StatusCode);
        }

        [Fact]
        public async Task DeleteChat_ValidRequest_DeletesChat()
        {
            await AuthorizeUserAsync();
            int chatToDeleteId = CreateTestChat();

            _ = await _httpClient.DeleteAsync($"/api/chat/delete/{chatToDeleteId}");

            Assert.False(_factory.DbContext.Chats.Any(x => x.Id == chatToDeleteId));
        }

        [Fact]
        public async Task JoinChat_Unauthorized_ReturnsUnauthorized()
        {
            HttpResponseMessage actual = await _httpClient.PostAsync("/api/chat/join/1", null);

            Assert.Equal(HttpStatusCode.Unauthorized, actual.StatusCode);
        }

        [Fact]
        public async Task JoinChat_NonExistingChat_ReturnsBadRequest()
        {
            await AuthorizeUserAsync();

            HttpResponseMessage actual = await _httpClient.PostAsync("/api/chat/join/10000", null);
        }

        [Fact]
        public async Task JoinChat_UserAlreadyInChat_ReturnsBadRequest()
        {
            await AuthorizeUserAsync();

            HttpResponseMessage actual = await _httpClient.PostAsync("/api/chat/join/1", null);

            Assert.Equal(HttpStatusCode.BadRequest, actual.StatusCode);
        }

        [Fact]
        public async Task JoinChat_UserAlreadyInChat_DoesntCreateNewChatMember()
        {
            await AuthorizeUserAsync();
            int expected = _factory.DbContext.ChatMembers.Count();

            _ = await _httpClient.PostAsync("/api/chat/join/1", null);

            int actual = _factory.DbContext.ChatMembers.Count();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task JoinChat_ValidRequest_ReturnsOk()
        {
            await AuthorizeUserAsync();

            HttpResponseMessage actual = await _httpClient.PostAsync("/api/chat/join/3", null);

            Assert.Equal(HttpStatusCode.OK, actual.StatusCode);

            ChatMemberCleanup(3);
        }

        [Fact]
        public async Task JoinChat_ValidRequest_CreatesNewChatMember()
        {
            await AuthorizeUserAsync();

            _ = await _httpClient.PostAsync("/api/chat/join/3", null);

            Assert.True(_factory.DbContext.ChatMembers.Any(x => x.ChatId == 3 && x.UserId == TestUserId));

            ChatMemberCleanup(3);
        }

        [Fact]
        public async Task LeaveChat_Unauthorized_ReturnsUnauthorized()
        {
            HttpResponseMessage actual = await _httpClient.PostAsync("/api/chat/leave/1", null);

            Assert.Equal(HttpStatusCode.Unauthorized, actual.StatusCode);
        }

        [Fact]
        public async Task LeaveChat_NonExistingChat_ReturnsBadRequest()
        {
            await AuthorizeUserAsync();

            HttpResponseMessage actual = await _httpClient.PostAsync("/api/chat/leave/1000", null);

            Assert.Equal(HttpStatusCode.BadRequest, actual.StatusCode);
        }

        [Fact]
        public async Task LeaveChat_UserNotInChat_ReturnBadRequest()
        {
            await AuthorizeUserAsync();

            HttpResponseMessage actual = await _httpClient.PostAsync("/api/chat/leave/3", null);

            Assert.Equal(HttpStatusCode.BadRequest, actual.StatusCode);
        }

        [Fact]
        public async Task LeaveChat_UserIsChatOwner_ReturnsBadRequest()
        {
            await AuthorizeUserAsync();

            HttpResponseMessage actual = await _httpClient.PostAsync("/api/chat/leave/1", null);

            Assert.Equal(HttpStatusCode.BadRequest, actual.StatusCode);
        }

        [Fact]
        public async Task LeaveChat_UserIsChatOwner_DoesntDeleteChatMember()
        {
            await AuthorizeUserAsync();

            _ = await _httpClient.PostAsync("/api/chat/leave/1", null);

            Assert.True(_factory.DbContext.ChatMembers.Any(x => x.ChatId == 1 && x.UserId == TestUserId));
        }

        [Fact]
        public async Task LeaveChat_ValidRequest_ReturnsOk()
        {
            await AuthorizeUserAsync();
            CreateTestChatMember();

            HttpResponseMessage actual = await _httpClient.PostAsync("/api/chat/leave/3", null);

            Assert.Equal(HttpStatusCode.OK, actual.StatusCode);
        }

        [Fact]
        public async Task LeaveChat_ValidRequest_DeletesChatMember()
        {
            await AuthorizeUserAsync();
            CreateTestChatMember();

            _ = await _httpClient.PostAsync("/api/chat/leave/3", null);

            Assert.False(_factory.DbContext.ChatMembers.Any(x => x.ChatId == 3 && x.UserId == TestUserId));
        }

        private void ChatCleanup(string chatName)
        {
            Chat? existingChat = _factory.DbContext.Chats
                .FirstOrDefault(x => x.Name == chatName);

            if (existingChat != null)
            {
                _factory.DbContext.Chats.Remove(existingChat);
                _factory.DbContext.SaveChanges();
            }
        }

        private void ChatMemberCleanup(int chatId)
        {
            ChatMember? existingMember = _factory.DbContext.ChatMembers
                .FirstOrDefault(x => x.ChatId == chatId && x.UserId == TestUserId);

            if (existingMember != null)
            {
                _factory.DbContext.ChatMembers.Remove(existingMember);
                _factory.DbContext.SaveChanges();
            }
        }

        private int CreateTestChat()
        {
            Chat newChat = new Chat() { Name = "testchat", ChatOwnerId = TestUserId };
            _factory.DbContext.Chats.Add(newChat);
            _factory.DbContext.SaveChanges();

            int newChatId = _factory.DbContext.Chats
                .First(x => x.Name == "testchat")
                .Id;

            return newChatId;
        }

        private void CreateTestChatMember()
        {
            ChatMember newMember = new ChatMember() { ChatId = 3, UserId = TestUserId };
            _factory.DbContext.ChatMembers.Add(newMember);
            _factory.DbContext.SaveChanges();
        }

        private List<ChatDto> GetUserChats_NoFilter_ExpectedChats()
        {
            return new List<ChatDto>()
            {
                new ChatDto { Id = 1, Name = "Owner", IsUserOwner = true },
                new ChatDto { Id = 2, Name = "Member", IsUserOwner = false },
                new ChatDto { Id = 3080, Name = "API", IsUserOwner = true },
                new ChatDto { Id = 3081, Name = "UnitTests", IsUserOwner = true },
                new ChatDto { Id = 3082, Name = "IntegrationTests", IsUserOwner = true },
                new ChatDto { Id = 3083, Name = "MAUI", IsUserOwner = true },
                new ChatDto { Id = 3084, Name = "MySQL", IsUserOwner = true },
            };
        }

        public static IEnumerable<object?[]> InvalidChatNames()
        {
            yield return new object?[] { null };
            yield return new object?[] { "sssssssssssssssssssssssssssssss" }; // Too long (31 character)
        }
    }
}
