using Auth.Dtos;
using Auth.Services;
using DataAccess.Entities;
using Geode.API;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.Json;
using Xunit;

namespace Geode.Api.IntegrationTests.Controllers
{
    public class UserControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _httpClient;
        private readonly CustomWebApplicationFactory<Program> _factory;

        public UserControllerTests(CustomWebApplicationFactory<Program> factory)
        {
            _httpClient = factory.CreateClient();
            _factory = factory;
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
                Email = "test@test.com",
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
                Email = "test@test.com",
                Password = "Passw0rd_",
            };

            HttpResponseMessage actual = await _httpClient.PostAsJsonAsync<LoginDto>("/api/user/login", loginDto);

            Assert.Equal(HttpStatusCode.OK, actual.StatusCode);
        }

        [Fact]
        public async Task Login_CorrectCredentials_ReturnsPairOfTokens()
        {
            LoginDto loginDto = new LoginDto()
            {
                Email = "test@test.com",
                Password = "Passw0rd_",
            };

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync<LoginDto>("/api/user/login", loginDto);
            string responseBody = await response.Content.ReadAsStringAsync();
            TokenDto? actual = JsonSerializer.Deserialize<TokenDto>(responseBody);

            Assert.NotNull(actual);
            Assert.NotNull(actual.AccessToken);
            Assert.NotNull(actual.RefreshToken);
        }

        [Fact]
        public async Task Register_CorrectDto_ReturnsOk()
        {
            RegisterDto registerDto = new RegisterDto()
            {
                Email = "tesemail@test.com",
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
                Email = "tesemail@test.com",
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
