using Application.Dtos;
using Application.Handlers.Messages;
using Application.Services.Messages;
using Application.Utils.Helpers.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Geode.Application.Tests.Handlers.Messages
{
    public class GetChatMessagesHandlerTests
    {
        private Mock<IChatRepositoryHelper> _repositoryHelperMock;

        public GetChatMessagesHandlerTests()
        {
            _repositoryHelperMock = new Mock<IChatRepositoryHelper>();
        }

        [Fact]
        public async Task Handle_Invocation_PassesExecutionToHelper()
        {
            GetChatMessagesQuery testQuery = new GetChatMessagesQuery(1);
            GetChatMessagesQueryHandler sut = new GetChatMessagesQueryHandler(_repositoryHelperMock.Object);

            _ = await sut.Handle(testQuery, CancellationToken.None);

            _repositoryHelperMock.Verify(x => x.GetMessagesInChat(1), Times.Once);
        }

        [Fact]
        public async Task Handle_Invocation_ReturnsResultFromHelper()
        {
            List<ChatMessageDto> expected = new List<ChatMessageDto>();
            GetChatMessagesQuery testQuery = new GetChatMessagesQuery(1);
            _repositoryHelperMock.Setup(x => x.GetMessagesInChat(1))
                .Returns(expected);
            GetChatMessagesQueryHandler sut = new GetChatMessagesQueryHandler(_repositoryHelperMock.Object);

            IEnumerable<ChatMessageDto> actual = await sut.Handle(testQuery, CancellationToken.None);

            Assert.Same(expected, actual);
        }
    }
}
