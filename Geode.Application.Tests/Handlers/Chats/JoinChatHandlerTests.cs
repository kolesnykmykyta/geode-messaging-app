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
    public class JoinChatHandlerTests
    {
        private Mock<IChatRepositoryHelper> _repositoryHelperMock;

        public JoinChatHandlerTests()
        {
            _repositoryHelperMock = new Mock<IChatRepositoryHelper>();
        }

        [Fact]
        public async Task Handle_Invocation_PassesExecutionToHelper()
        {
            JoinChatCommand testCommand = new JoinChatCommand() { ChatId = 1, UserId = "test" };
            JoinChatCommandHandler sut = new JoinChatCommandHandler(_repositoryHelperMock.Object);

            _ = await sut.Handle(testCommand, CancellationToken.None);

            _repositoryHelperMock.Verify(x => x.JoinChat(testCommand.ChatId, testCommand.UserId), Times.Once);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task Handle_Invocation_ReturnsResultFromHelper(bool expected)
        {
            JoinChatCommand testCommand = new JoinChatCommand() { ChatId = 1, UserId = "test" };
            _repositoryHelperMock.Setup(x => x.JoinChat(testCommand.ChatId, testCommand.UserId))
                .Returns(expected);
            JoinChatCommandHandler sut = new JoinChatCommandHandler(_repositoryHelperMock.Object);

            bool actual = await sut.Handle(testCommand, CancellationToken.None);

            Assert.Equal(expected, actual);
        }
    }
}
