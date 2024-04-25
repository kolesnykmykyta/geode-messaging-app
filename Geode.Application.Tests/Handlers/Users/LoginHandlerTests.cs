using Application.Handlers.Users;
using Application.Services.Users;
using Auth.Dtos;
using Auth.Services;
using Auth.Services.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core.Tokenizer;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Geode.Application.Tests.Handlers.Users
{
    public class LoginHandlerTests
    {
        [Fact]
        public async Task Handle_Invocation_ReturnsResultFromAuthService()
        {
            TokenDto? expected = new TokenDto();
            LoginDto testDto = new LoginDto();
            Mock<IAuthService> authServiceMock = new Mock<IAuthService>();
            authServiceMock.Setup(x => x.LoginAsync(testDto))
                .ReturnsAsync(expected);
            LoginQueryHandler sut = new LoginQueryHandler(authServiceMock.Object);

            TokenDto? actual = await sut.Handle(new LoginQuery { Dto = testDto}, CancellationToken.None);

            Assert.Same(expected, actual);
        }
    }
}
