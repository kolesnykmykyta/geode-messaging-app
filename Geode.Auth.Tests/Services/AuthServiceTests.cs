using Auth.Dtos;
using Auth.Services;
using DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
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
        private readonly Mock<UserManager<User>> _userManagerMock;
        private readonly Mock<IConfiguration> _configMock;

        private const string testAccessToken = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJ0ZXN0QHRlc3QuY29tIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZSI6InRlc3QiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6ImJjMWU0ZWEwLTFhYjMtNDU2NC1hMWI3LWUyZTVmNmMwNzRkOCIsImV4cCI6MTcxNDE0Nzg1MywiaXNzIjoiVGVzdElzc3VlciIsImF1ZCI6IlRlc3RBdWRpZW5jZSJ9.IcV4HUHUf0La3hnP0pqeRV0zlZS4EZdemuB_yNQuqdA";

        public AuthServiceTests()
        {
            var store = new Mock<IUserStore<User>>();
            _userManagerMock = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);

            _configMock = new Mock<IConfiguration>();
        }

        [Fact]
        [Trait("Category", "LoginAsync")]
        public async Task LoginAsync_NonExistingUser_ReturnsNull()
        {
            _userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((User)null);
            AuthService sut = new AuthService(_userManagerMock.Object, _configMock.Object);

            TokenDto? actual = await sut.LoginAsync(new LoginDto() { Email = "test", Password = "test" });

            Assert.Null(actual);
        }

        [Fact]
        [Trait("Category", "LoginAsync")]
        public async Task LoginAsync_WrongPassword_ReturnsNull()
        {
            _userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(new User());
            _userManagerMock.Setup(x => x.CheckPasswordAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(false);
            AuthService sut = new AuthService(_userManagerMock.Object, _configMock.Object);

            TokenDto? actual = await sut.LoginAsync(new LoginDto() { Email = "test", Password = "test" });

            Assert.Null(actual);
        }

        [Fact]
        [Trait("Category", "LoginAsync")]
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
        [Trait("Category", "LoginAsync")]
        public async Task LoginAsync_CorrectCredentials_UpdatesUserRefreshToken()
        {
            PrepareMocksForLoginTest();
            AuthService sut = new AuthService(_userManagerMock.Object, _configMock.Object);

            _ = await sut.LoginAsync(new LoginDto() { Email = "test", Password = "test" });

            _userManagerMock.Verify(x => x.UpdateAsync(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        [Trait("Category", "LoginAsync")]
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

        [Fact]
        [Trait("Category", "RegisterAsync")]
        public async Task RegisterAsync_SuccessCreation_ReturnsPositiveResult()
        {
            IdentityResult mockResult = IdentityResult.Success;
            _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(mockResult);
            AuthService sut = new AuthService(_userManagerMock.Object, null);

            RegisterResultDto actual = await sut.RegisterAsync(new RegisterDto());

            Assert.True(actual.IsSuccess);
            Assert.Null(actual.Errors);
        }

        [Fact]
        [Trait("Category", "RegisterAsync")]
        public async Task RegisterAsync_FailedCreation_ReturnsNegativeResult()
        {
            IdentityResult mockResult = IdentityResult.Failed();
            _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(mockResult);
            AuthService sut = new AuthService(_userManagerMock.Object, null);

            RegisterResultDto actual = await sut.RegisterAsync(new RegisterDto());

            Assert.False(actual.IsSuccess);
        }

        [Fact]
        [Trait("Category", "RegisterAsync")]
        public async Task RegisterAsync_FailedCreation_ReturnsAllErrors()
        {
            List<IdentityError> mockErrors = new List<IdentityError>
            {
                new IdentityError { Code = "Test1", Description = "Test error 1" },
                new IdentityError { Code = "Test2", Description = "Test error 2" }
            };
            IdentityResult mockResult = IdentityResult.Failed(mockErrors.ToArray());
            _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(mockResult);
            AuthService sut = new AuthService(_userManagerMock.Object, null);

            RegisterResultDto actual = await sut.RegisterAsync(new RegisterDto());

            Assert.NotNull(actual.Errors);
            Assert.Equal(mockErrors.Count, actual.Errors.Count());
            Assert.Equal(mockErrors[0].Description, actual.Errors.ToList()[0]);
            Assert.Equal(mockErrors[1].Description, actual.Errors.ToList()[1]);
        }

        [Fact]
        public async Task RefreshAsync_NoUserInToken_ReturnsNull()
        {
            PrepareConfigurationMock();
            AuthService sut = new AuthService(_userManagerMock.Object, _configMock.Object);

            TokenDto? actual = await sut.RefreshAsync(new TokenDto 
            {
                AccessToken = testAccessToken,
            });

            Assert.Null(actual);
        }

        [Fact]
        public async Task RefreshAsync_UserRefreshTokenDiffers_ReturnsNull()
        {
            PrepareConfigurationMock();
            _userManagerMock.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(new User { RefreshToken = "refresh1" });
            AuthService sut = new AuthService(_userManagerMock.Object, _configMock.Object);

            TokenDto? actual = await sut.RefreshAsync(new TokenDto 
            {
                AccessToken = testAccessToken,
                RefreshToken = "refresh2"
            });

            Assert.Null(actual);
        }

        [Fact]
        public async Task RefreshAsync_TokenIsExpired_ReturnsNull()
        {
            PrepareConfigurationMock();
            _userManagerMock.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(new User { RefreshToken = "refresh1", RefreshTokenExpirationDate = DateTime.MinValue });
            AuthService sut = new AuthService(_userManagerMock.Object, _configMock.Object);

            TokenDto? actual = await sut.RefreshAsync(new TokenDto
            {
                AccessToken = testAccessToken,
                RefreshToken = "refresh1"
            });

            Assert.Null(actual);
        }

        [Fact]
        public async Task RefreshAsync_ValidRequest_ReturnsNewTokens()
        {
            PrepareConfigurationMock();
            _userManagerMock.Setup(x => x.GetRolesAsync(It.IsAny<User>()))
                .ReturnsAsync(new List<string>());
            _userManagerMock.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(new User
                {
                    RefreshToken = "refresh1",
                    RefreshTokenExpirationDate = DateTime.Now.AddDays(1),
                    Email = "test",
                    UserName = "test",
                    Id = "test",
                });
            AuthService sut = new AuthService(_userManagerMock.Object, _configMock.Object);

            TokenDto? actual = await sut.RefreshAsync(new TokenDto
            {
                AccessToken = testAccessToken,
                RefreshToken = "refresh1"
            });

            Assert.NotNull(actual);
            Assert.NotNull(actual.AccessToken);
            Assert.NotNull(actual.RefreshToken);
        }

        [Fact]
        public async Task RefreshAsync_ValidRequest_UpdatesRefreshToken()
        {
            PrepareConfigurationMock();
            _userManagerMock.Setup(x => x.GetRolesAsync(It.IsAny<User>()))
                .ReturnsAsync(new List<string>());
            _userManagerMock.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(new User
                {
                    RefreshToken = "refresh1",
                    RefreshTokenExpirationDate = DateTime.Now.AddDays(1),
                    Email = "test",
                    UserName = "test",
                    Id = "test",
                });
            AuthService sut = new AuthService(_userManagerMock.Object, _configMock.Object);

            _ = await sut.RefreshAsync(new TokenDto
            {
                AccessToken = testAccessToken,
                RefreshToken = "refresh1"
            });

            _userManagerMock.Verify(x => x.UpdateAsync(It.IsAny<User>()), Times.Once);
        }

        private void PrepareMocksForLoginTest()
        {
            _userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(new User() { UserName = "TestUser", Email = "TestEmail", Id = "TestId" });
            _userManagerMock.Setup(x => x.CheckPasswordAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(true);
            _userManagerMock.Setup(x => x.GetRolesAsync(It.IsAny<User>()))
                .ReturnsAsync(new List<string>());

            PrepareConfigurationMock();
        }

        private void PrepareConfigurationMock()
        {
            _configMock.Setup(x => x.GetSection("Jwt:Key").Value)
                .Returns("TestKeyTestKeyTestKeyTestKeyTestKey");
            _configMock.Setup(x => x.GetSection("Jwt:Issuer").Value)
                .Returns("TestIssuer");
            _configMock.Setup(x => x.GetSection("Jwt:Audience").Value)
                .Returns("TestAudience");
        }
    }
}
