using Application.Handlers.Chats;
using Application.Services.Chats;
using Application.Utils.Helpers.Interfaces;
using AutoMapper;
using DataAccess.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Geode.Application.Tests.Handlers.Chats
{
    public class CreateChatHandlerTests
    {
        private readonly Mock<IChatRepositoryHelper> _repositoryHelperMock;
        private readonly Mock<IMapper> _mapperMock;

        public CreateChatHandlerTests()
        {
            _repositoryHelperMock = new Mock<IChatRepositoryHelper>();
            _mapperMock = new Mock<IMapper>();
        }

        [Fact]
        public async Task Handle_AnyMessage_PassesExecutionToHelper()
        {
            string testName = "Test";
            string testOwner = "Owner";
            CreateChatCommand testCommand = new CreateChatCommand() { Name = testName, ChatOwnerId = testOwner };
            Chat testChat = new Chat {  Name = testName, ChatOwnerId = testOwner };
            _mapperMock.Setup(x => x.Map<Chat>(testCommand))
                .Returns(testChat);
            CreateChatCommandHandler sut = new CreateChatCommandHandler(_mapperMock.Object, _repositoryHelperMock.Object);

            await sut.Handle(testCommand, CancellationToken.None);

            _repositoryHelperMock.Verify(x => x.CreateNewChat(testChat), Times.Once);
        }
    }
}
