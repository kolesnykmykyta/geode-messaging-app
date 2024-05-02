using Auth.Dtos;
using Auth.Services;
using DataAccess.Entities;
using Geode.API;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.Json;
using Xunit;

namespace Geode.Api.IntegrationTests.Controllers
{
    public class UserControllerTests : BaseTestClass
    {
        public UserControllerTests(CustomWebApplicationFactory<Program> factory) : base(factory)
        {
        }

        [Fact]
        public async Task Login_NonExistingUser_ReturnsBadRequest()
        {
            LoginDto loginDto = new LoginDto()
            {
                Email = "nonexisting",
                Password = "wrong",
            };

            HttpResponseMessage actual = await _httpClient.PostAsJsonAsync<LoginDto>("/api/user/login", loginDto);

            Assert.Equal(HttpStatusCode.BadRequest, actual.StatusCode);
        }

        [Fact]
        public async Task Login_ExistingUserWithWrongPassword_ReturnsBadRequest()
        {
            LoginDto loginDto = new LoginDto()
            {
                Email = TestUserEmail,
                Password = "wrong",
            };

            HttpResponseMessage actual = await _httpClient.PostAsJsonAsync<LoginDto>("/api/user/login", loginDto);

            Assert.Equal(HttpStatusCode.BadRequest, actual.StatusCode);
        }

        [Fact]
        public async Task Login_CorrectCredentials_ReturnsOk()
        {
            LoginDto loginDto = new LoginDto()
            {
                Email = TestUserEmail,
                Password = TestUserPassword,
            };

            HttpResponseMessage actual = await _httpClient.PostAsJsonAsync<LoginDto>("/api/user/login", loginDto);

            Assert.Equal(HttpStatusCode.OK, actual.StatusCode);
        }

        [Fact]
        public async Task Login_CorrectCredentials_ReturnsPairOfTokens()
        {
            LoginDto loginDto = new LoginDto()
            {
                Email = TestUserEmail,
                Password = TestUserPassword,
            };

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync<LoginDto>("/api/user/login", loginDto);
            string responseBody = await response.Content.ReadAsStringAsync();
            TokenDto? actual = JsonSerializer.Deserialize<TokenDto>(responseBody);

            Assert.NotNull(actual);
            Assert.NotNull(actual.AccessToken);
            Assert.NotNull(actual.RefreshToken);
        }

        [Fact]
        public async Task Login_CorrectCredentials_UpdatesRefreshToken()
        {
            User testUser = _factory.DbContext.Users
                .AsNoTracking()
                .First(u => u.UserName == TestUserName);
            string oldToken = testUser.RefreshToken!;

            LoginDto loginDto = new LoginDto()
            {
                Email = TestUserEmail,
                Password = TestUserPassword,
            };

            _ = await _httpClient.PostAsJsonAsync<LoginDto>("/api/user/login", loginDto);

            testUser = _factory.DbContext.Users
                .AsNoTracking()
                .First(u => u.UserName == TestUserName);
            string newToken = testUser.RefreshToken!;

            Assert.NotEqual(oldToken, newToken);
        }

        [Fact]
        public async Task Login_CorrectCredentials_UpdatesRefreshTokenExpiry()
        {
            User testUser = _factory.DbContext.Users
                .AsNoTracking()
                .First(u => u.UserName == TestUserName);
            DateTime? oldExpiry = testUser.RefreshTokenExpirationDate;

            LoginDto loginDto = new LoginDto()
            {
                Email = TestUserPassword,
                Password = TestUserPassword,
            };

            _ = await _httpClient.PostAsJsonAsync<LoginDto>("/api/user/login", loginDto);

            testUser = _factory.DbContext.Users
                .AsNoTracking()
                .First(u => u.UserName == TestUserName);
            DateTime? newExpiry = testUser.RefreshTokenExpirationDate;

            Assert.True(oldExpiry <= newExpiry);
        }

        [Fact]
        public async Task Register_CorrectDto_ReturnsOk()
        {
            RegisterDto registerDto = new RegisterDto()
            {
                Email = "testemail@test.com",
                Username = "testusername",
                Password = "Passw0rd_"
            };

            HttpResponseMessage actual = await _httpClient.PostAsJsonAsync("/api/user/register", registerDto);

            Assert.Equal(HttpStatusCode.OK, actual.StatusCode);

            RegistrationTestCleanup("testusername");
        }

        [Fact]
        public async Task Register_CorrectDto_CreatesUserInDatabase()
        {
            RegisterDto registerDto = new RegisterDto()
            {
                Email = "testemail@test.com",
                Username = "testusername",
                Password = "Passw0rd_"
            };

            _ = await _httpClient.PostAsJsonAsync("/api/user/register", registerDto);

            Assert.True(_factory.DbContext.Users.Any(u => u.UserName == "testusername"));

            RegistrationTestCleanup("testusername");
        }

        [Theory]
        [MemberData(nameof(InvalidEmails))]
        public async Task Register_InvalidEmail_ReturnsBadRequest(string email)
        {
            RegisterDto registerDto = new RegisterDto()
            {
                Email = email,
                Username = "testusername",
                Password = "Passw0rd_"
            };

            HttpResponseMessage actual = await _httpClient.PostAsJsonAsync("/api/user/register", registerDto);

            Assert.Equal(HttpStatusCode.BadRequest, actual.StatusCode);
        }

        [Theory]
        [MemberData(nameof(InvalidEmails))]
        public async Task Register_InvalidEmail_DoesNothing(string email)
        {
            int expected = _factory.DbContext.Users.Count();

            RegisterDto registerDto = new RegisterDto()
            {
                Email = email,
                Username = "testusername",
                Password = "Passw0rd_"
            };

            _ = await _httpClient.PostAsJsonAsync("/api/user/register", registerDto);
            int actual = _factory.DbContext.Users.Count();

            Assert.Equal(expected, actual);
        }

        [Theory]
        [MemberData(nameof(InvalidUsernames))]
        public async Task Register_InvalidUsername_ReturnsBadRequest(string username)
        {
            RegisterDto registerDto = new RegisterDto()
            {
                Email = "testemail@email.com",
                Username = username,
                Password = "Passw0rd_"
            };

            HttpResponseMessage actual = await _httpClient.PostAsJsonAsync("/api/user/register", registerDto);

            Assert.Equal(HttpStatusCode.BadRequest, actual.StatusCode);
        }

        [Theory]
        [MemberData(nameof(InvalidUsernames))]
        public async Task Register_InvalidUsername_DoesNothing(string username)
        {
            int expected = _factory.DbContext.Users.Count();

            RegisterDto registerDto = new RegisterDto()
            {
                Email = "testemail@test.com",
                Username = username,
                Password = "Passw0rd_"
            };

            _ = await _httpClient.PostAsJsonAsync("/api/user/register", registerDto);
            int actual = _factory.DbContext.Users.Count();

            Assert.Equal(expected, actual);
        }

        [Theory]
        [MemberData(nameof(InvalidPasswords))]
        public async Task Register_InvalidPassword_ReturnsBadRequest(string password)
        {
            RegisterDto registerDto = new RegisterDto()
            {
                Email = "testemail@email.com",
                Username = "testusername",
                Password = password,
            };

            HttpResponseMessage actual = await _httpClient.PostAsJsonAsync("/api/user/register", registerDto);

            Assert.Equal(HttpStatusCode.BadRequest, actual.StatusCode);
        }

        [Theory]
        [MemberData(nameof(InvalidPasswords))]
        public async Task Register_InvalidPassword_DoesNothing(string password)
        {
            int expected = _factory.DbContext.Users.Count();

            RegisterDto registerDto = new RegisterDto()
            {
                Email = "testemail@email.com",
                Username = "testusername",
                Password = password,
            };

            _ = await _httpClient.PostAsJsonAsync("/api/user/register", registerDto);
            int actual = _factory.DbContext.Users.Count();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task Refresh_NonExistingUser_ReturnsBadRequest()
        {
            TokenDto testDto = new TokenDto()
            {
                AccessToken = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJUT0RFTEVURUB0ZXN0LmNvbSIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiJUT0RFTEVURSIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWVpZGVudGlmaWVyIjoiYzMwMjRhMTItMjE5ZS00ZjBhLWI3MTQtMTY0NWNhYzU5OGU2IiwiZXhwIjoxNzE0NTQ5ODg3LCJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo3MDc3IiwiYXVkIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NzA3NyJ9._lV06snngwiI61dGen5MvntCGsxJr57ArBiMHczAZtM",
                RefreshToken = "RNg0mU0nIzRnkGomX1Rf6QpCl3YS6xzNcuL/dIxmPC0=",
            };

            HttpResponseMessage actual = await _httpClient.PostAsJsonAsync("/api/user/refresh", testDto);

            Assert.Equal(HttpStatusCode.BadRequest, actual.StatusCode);
        }

        [Fact]
        public async Task Refresh_ExpiredToken_ReturnsBadRequest()
        {
            TokenDto testDto = new TokenDto()
            {
                AccessToken = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJleHBpcmVkdG9rZW5AdGVzdC5jb20iLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiZXhwaXJlZHRva2VuIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZWlkZW50aWZpZXIiOiI4NzdhNDZhOS0zYmZhLTQwY2EtOGFkOS1jMDIyYzAxYjMxMjAiLCJleHAiOjE3MTQ1NDk5MjMsImlzcyI6Imh0dHBzOi8vbG9jYWxob3N0OjcwNzciLCJhdWQiOiJodHRwczovL2xvY2FsaG9zdDo3MDc3In0._CrF4GHD0VyJh2rgZNBl3OEJtIY6dmWKUmvvIFzreyc",
                RefreshToken = "kava0pGkyUeXQGPCyWBTzIsZkD/uLTUdjLzl4xqgqEI=",
            };

            HttpResponseMessage actual = await _httpClient.PostAsJsonAsync("/api/user/refresh", testDto);

            Assert.Equal(HttpStatusCode.BadRequest, actual.StatusCode);
        }

        [Fact]
        public async Task Refresh_ValidToken_ReturnsOk()
        {
            User testUser = _factory.DbContext.Users
                .AsNoTracking()
                .First(u => u.UserName == "validrefresh");
            TokenDto testDto = new TokenDto()
            {
                AccessToken = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJ2YWxpZHJlZnJlc2hAdGVzdC5jb20iLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoidmFsaWRyZWZyZXNoIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZWlkZW50aWZpZXIiOiJiYzA3NTE5YS1kYjE3LTQ5MDQtYmUxOS03YzE2OTQwZWJiYzUiLCJleHAiOjE3MTQ1NTIyMjEsImlzcyI6Imh0dHBzOi8vbG9jYWxob3N0OjcwNzciLCJhdWQiOiJodHRwczovL2xvY2FsaG9zdDo3MDc3In0.DQsK0WmaZrqySLvzfggpQ6fqJzRJIrDGkicfIPALs7Y",
                RefreshToken = testUser.RefreshToken!,
            };

            HttpResponseMessage actual = await _httpClient.PostAsJsonAsync("/api/user/refresh", testDto);

            Assert.Equal(HttpStatusCode.OK, actual.StatusCode);
        }

        [Fact]
        public async Task Refresh_ValidToken_ReturnsNewPairOfTokens()
        {
            User testUser = _factory.DbContext.Users
                .AsNoTracking()
                .First(u => u.UserName == "validrefresh");
            TokenDto testDto = new TokenDto()
            {
                AccessToken = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJ2YWxpZHJlZnJlc2hAdGVzdC5jb20iLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoidmFsaWRyZWZyZXNoIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZWlkZW50aWZpZXIiOiJiYzA3NTE5YS1kYjE3LTQ5MDQtYmUxOS03YzE2OTQwZWJiYzUiLCJleHAiOjE3MTQ1NTIyMjEsImlzcyI6Imh0dHBzOi8vbG9jYWxob3N0OjcwNzciLCJhdWQiOiJodHRwczovL2xvY2FsaG9zdDo3MDc3In0.DQsK0WmaZrqySLvzfggpQ6fqJzRJIrDGkicfIPALs7Y",
                RefreshToken = testUser.RefreshToken!,
            };

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("/api/user/refresh", testDto);
            string responseBody = await response.Content.ReadAsStringAsync();
            TokenDto? actual = JsonSerializer.Deserialize<TokenDto>(responseBody);

            Assert.NotNull(actual);
            Assert.NotNull(actual.RefreshToken);
            Assert.NotNull(actual.AccessToken);
        }

        [Fact]
        public async Task Refresh_ValidToken_ChangesRefreshToken()
        {
            User testUser = _factory.DbContext.Users
                .AsNoTracking()
                .First(u => u.UserName == "validrefresh");

            string oldToken = testUser.RefreshToken!;
            TokenDto testDto = new TokenDto()
            {
                AccessToken = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJ2YWxpZHJlZnJlc2hAdGVzdC5jb20iLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoidmFsaWRyZWZyZXNoIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZWlkZW50aWZpZXIiOiJiYzA3NTE5YS1kYjE3LTQ5MDQtYmUxOS03YzE2OTQwZWJiYzUiLCJleHAiOjE3MTQ1NTIyMjEsImlzcyI6Imh0dHBzOi8vbG9jYWxob3N0OjcwNzciLCJhdWQiOiJodHRwczovL2xvY2FsaG9zdDo3MDc3In0.DQsK0WmaZrqySLvzfggpQ6fqJzRJIrDGkicfIPALs7Y",
                RefreshToken = testUser.RefreshToken!,
            };

            _ = await _httpClient.PostAsJsonAsync("/api/user/refresh", testDto);
            User updatedUser = _factory.DbContext.Users
                .First(u => u.UserName == "validrefresh");
            string newToken = updatedUser.RefreshToken!;

            Assert.NotEqual(oldToken, newToken);
        }

        [Fact]
        public async Task Refresh_ValidToken_DoesntChangeRefreshTokenExpiry()
        {
            User testUser = _factory.DbContext.Users
                .AsNoTracking()
                .First(u => u.UserName == "validrefresh");
            DateTime? oldExpiry = testUser.RefreshTokenExpirationDate;
            TokenDto testDto = new TokenDto()
            {
                AccessToken = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJ2YWxpZHJlZnJlc2hAdGVzdC5jb20iLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoidmFsaWRyZWZyZXNoIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZWlkZW50aWZpZXIiOiJiYzA3NTE5YS1kYjE3LTQ5MDQtYmUxOS03YzE2OTQwZWJiYzUiLCJleHAiOjE3MTQ1NTIyMjEsImlzcyI6Imh0dHBzOi8vbG9jYWxob3N0OjcwNzciLCJhdWQiOiJodHRwczovL2xvY2FsaG9zdDo3MDc3In0.DQsK0WmaZrqySLvzfggpQ6fqJzRJIrDGkicfIPALs7Y",
                RefreshToken = testUser.RefreshToken!,
            };

            _ = await _httpClient.PostAsJsonAsync("/api/user/refresh", testDto);
            testUser = _factory.DbContext.Users
                .First(u => u.UserName == "validrefresh");
            DateTime? newExpiry = testUser.RefreshTokenExpirationDate;

            Assert.Equal(oldExpiry, newExpiry);
        }

        private void RegistrationTestCleanup(string username)
        {
            User? userAfterTest = _factory.DbContext.Users
                .Where(u => u.UserName == username)
                .FirstOrDefault();

            if (userAfterTest != null)
            {
                _factory.DbContext.Users.Remove(userAfterTest);
                _factory.DbContext.SaveChanges();
            }
        }

        public static IEnumerable<object?[]> InvalidEmails()
        {
            yield return new object?[] { null };
            yield return new object?[] { string.Empty };
            yield return new object?[] { "s" };
            yield return new object?[] { "@" };
            yield return new object?[] { "test@test.com" };  // Valid, but already taken
        }

        public static IEnumerable<object?[]> InvalidUsernames()
        {
            yield return new object?[] { null };
            yield return new object?[] { string.Empty };
            yield return new object?[] { "test" }; // Valid, but already taken
        }

        public static IEnumerable<object?[]> InvalidPasswords()
        {
            yield return new object?[] { null };
            yield return new object?[] { string.Empty };
            yield return new object?[] { "stringg1!" }; // No uppercase characters
            yield return new object?[] { "Stringg!" }; // No numeric characters
            yield return new object?[] { "String1" }; // No not numeric/alphabetic characters
            yield return new object?[] { "Sg1!" }; // Too short password
            yield return new object?[] { "123456789" }; // No alphabetic characters
        }
    }
}
