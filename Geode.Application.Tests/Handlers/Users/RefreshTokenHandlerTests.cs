using Application.Handlers.Users;
using Application.Services.Users;
using Auth.Dtos;
using Auth.Services;
using Auth.Services.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Geode.Application.Tests.Handlers.Users
{
    public class RefreshTokenHandlerTests
    {
        [Fact]
        public async Task Handle_Invocation_ReturnsResultFromAuthService()
        {
            TokenDto testDto = new TokenDto();
            TokenDto expected = new TokenDto();
            Mock<IAuthService> authServiceMock = new Mock<IAuthService>();
            authServiceMock.Setup(x => x.RefreshAsync(testDto))
                .ReturnsAsync(expected);
            RefreshTokenQueryHandler sut = new RefreshTokenQueryHandler(authServiceMock.Object);

            TokenDto? actual = await sut.Handle(new RefreshTokenQuery { Dto = testDto }, CancellationToken.None);

            Assert.Same(expected, actual);
        }
    }
}
