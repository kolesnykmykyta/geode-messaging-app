using Auth.Dtos;
using Auth.Services;
using DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Geode.Auth.Tests.Services
{
    public class AuthServiceTests
    {
        Mock<UserManager<User>> _userManagerMock;
        Mock<IConfiguration> _configMock;

        public AuthServiceTests()
        {
            var store = new Mock<IUserStore<User>>();
            _userManagerMock = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);

            _configMock = new Mock<IConfiguration>();
        }

        [Fact]
        public async Task LoginAsync_NonExistingUser_ReturnsNull()
        {
            _userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .Returns(Task.FromResult<User>(null));
            AuthService sut = new AuthService(_userManagerMock.Object, _configMock.Object);

            TokenDto? actual = await sut.LoginAsync(new LoginDto() { Email = "test", Password = "test" });

            Assert.Null(actual);
        }

        [Fact]
        public async Task LoginAsync_WrongPassword_ReturnsNull()
        {
            _userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(new User()));
            _userManagerMock.Setup(x => x.CheckPasswordAsync(It.IsAny<User>(), It.IsAny<string>()))
                .Returns(Task.FromResult(false));
            AuthService sut = new AuthService(_userManagerMock.Object, _configMock.Object);

            TokenDto? actual = await sut.LoginAsync(new LoginDto() { Email = "test", Password = "test" });

            // Should I also verify mock methods invocation?
            Assert.Null(actual);
        }

        [Fact]
        public async Task LoginAsync_CorrectCredentials_ReturnsTokens()
        {
            PrepareMocksForLoginTest();
            AuthService sut = new AuthService(_userManagerMock.Object, _configMock.Object);

            TokenDto? actual = await sut.LoginAsync(new LoginDto() { Email = "test", Password = "test" });

            Assert.NotNull(actual);
            Assert.NotNull(actual.AccessToken);
            Assert.NotNull(actual.RefreshToken);
        }

        [Fact]
        public async Task LoginAsync_CorrectCredentials_UpdatesUserRefreshToken()
        {
            PrepareMocksForLoginTest();
            AuthService sut = new AuthService(_userManagerMock.Object, _configMock.Object);

            _ = await sut.LoginAsync(new LoginDto() { Email = "test", Password = "test" });

            _userManagerMock.Verify(x => x.UpdateAsync(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async Task LoginAsync_CorrectCredentials_ReturnsTokenWithClaims()
        {
            PrepareMocksForLoginTest();
            AuthService sut = new AuthService(_userManagerMock.Object, _configMock.Object);
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            string expectedId = "TestId";
            string expectedEmail = "TestEmail";
            string expectedUsername = "TestUser";

            TokenDto? actual = await sut.LoginAsync(new LoginDto() { Email = "test", Password = "test" });

            JwtSecurityToken actualToken = handler.ReadJwtToken(actual!.AccessToken);
            string? actualId = actualToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            string? actualEmail = actualToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            string? actualUsername = actualToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

            Assert.Equal(expectedId, actualId);
            Assert.Equal(expectedEmail, actualEmail);
            Assert.Equal(expectedUsername, actualUsername);
        }

        private void PrepareMocksForLoginTest()
        {
            _userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(new User() { UserName = "TestUser", Email = "TestEmail", Id = "TestId" }));
            _userManagerMock.Setup(x => x.CheckPasswordAsync(It.IsAny<User>(), It.IsAny<string>()))
                .Returns(Task.FromResult(true));
            _userManagerMock.Setup(x => x.GetRolesAsync(It.IsAny<User>()))
                .Returns(Task.FromResult<IList<string>>(new List<string>()));

            _configMock.Setup(x => x.GetSection("Jwt:Key").Value)
                .Returns("TestKeyTestKeyTestKeyTestKeyTestKey");
            _configMock.Setup(x => x.GetSection("Jwt:Issuer").Value)
                .Returns("TestIssuer");
            _configMock.Setup(x => x.GetSection("Jwt:Audience").Value)
                .Returns("TestAudience");
        }
    }
}
