using Application.Handlers.Users;
using Application.Services.Users;
using Auth.Dtos;
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
    public class RegisterNewUserHandlerTests
    {
        [Fact]
        public async Task Handle_Invocation_ReturnsResultFromAuthService()
        {
            RegisterDto testDto = new RegisterDto();
            RegisterResultDto expected = new RegisterResultDto(true);
            Mock<IAuthService> authServiceMock = new Mock<IAuthService>();
            authServiceMock.Setup(x => x.RegisterAsync(testDto))
                .ReturnsAsync(expected);
            RegisterNewUserCommandHandler sut = new RegisterNewUserCommandHandler(authServiceMock.Object);

            RegisterResultDto? actual = await sut.Handle(new RegisterNewUserCommand { Dto = testDto }, CancellationToken.None);

            Assert.Same(expected, actual);
        }
    }
}
