using Application.Handlers.Chats;
using Application.Services.Chats;
using Application.Utils.Helpers.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Geode.Application.Tests.Handlers.Chats
{
    public class LeaveChatHandlerTests
    {
        private Mock<IChatRepositoryHelper> _repositoryHelperMock;

        public LeaveChatHandlerTests()
        {
            _repositoryHelperMock = new Mock<IChatRepositoryHelper>();
        }

        [Fact]
        public async Task Handler_Invocation_PassesExectutionToHelper()
        {
            LeaveChatCommand testCommand = new LeaveChatCommand() { ChatId = 1, UserId = "test" };
            LeaveChatCommandHandler sut = new LeaveChatCommandHandler(_repositoryHelperMock.Object);

            _ = await sut.Handle(testCommand, CancellationToken.None);

            _repositoryHelperMock.Verify(x => x.LeaveChat(testCommand.ChatId, testCommand.UserId), Times.Once);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task Handler_Invocation_ReturnsResultFromHelper(bool expected)
        {
            LeaveChatCommand testCommand = new LeaveChatCommand() { ChatId = 1, UserId = "test" };
            _repositoryHelperMock.Setup(x => x.LeaveChat(testCommand.ChatId, testCommand.UserId))
                .Returns(expected);
            LeaveChatCommandHandler sut = new LeaveChatCommandHandler(_repositoryHelperMock.Object);

            bool actual = await sut.Handle(testCommand, CancellationToken.None);

            Assert.Equal(expected, actual);
        }
    }
}
