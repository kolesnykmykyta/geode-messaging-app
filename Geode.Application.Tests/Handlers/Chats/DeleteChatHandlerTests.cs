using Application.Handlers.Chats;
using Application.Services.Chats;
using DataAccess.Entities;
using DataAccess.UnitOfWork;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Geode.Application.Tests.Handlers.Chats
{
    public class DeleteChatHandlerTests
    {
        private readonly Mock<IUnitOfWork> _uowMock;

        public DeleteChatHandlerTests()
        {
            _uowMock = new Mock<IUnitOfWork>();
        }

        [Fact]
        public async Task Handle_NonExistingChat_ReturnsFalse()
        {
            _uowMock.Setup(x => x.GenericRepository<Chat>().GetById(It.IsAny<int>()))
                .Returns((Chat?)null);
            DeleteChatCommandHandler sut = new DeleteChatCommandHandler(_uowMock.Object);

            bool actual = await sut.Handle(new DeleteChatCommand(), CancellationToken.None);

            Assert.False(actual);
        }

        [Fact]
        public async Task Handle_UserIsNotOwner_ReturnsFalse()
        {
            Chat testChat = GenerateChatForTest();
            DeleteChatCommand testCommand = GenerateCommandForTest(userId: "user2");
            _uowMock.Setup(x => x.GenericRepository<Chat>().GetById(It.IsAny<int>()))
                .Returns(testChat);
            DeleteChatCommandHandler sut = new DeleteChatCommandHandler(_uowMock.Object);

            bool actual = await sut.Handle(testCommand, CancellationToken.None);

            Assert.False(actual);
        }

        [Fact]
        public async Task Handle_ValidRequest_ReturnsTrue()
        {
            Chat testChat = GenerateChatForTest();
            DeleteChatCommand testCommand = GenerateCommandForTest();
            _uowMock.Setup(x => x.GenericRepository<Chat>().GetById(It.IsAny<int>()))
                            .Returns(testChat);
            DeleteChatCommandHandler sut = new DeleteChatCommandHandler(_uowMock.Object);

            bool actual = await sut.Handle(testCommand, CancellationToken.None);

            Assert.True(actual);
        }

        [Fact]
        public async Task Handle_ValidRequest_DeletesInDatabase()
        {
            Chat testChat = GenerateChatForTest();
            DeleteChatCommand testCommand = GenerateCommandForTest();
            _uowMock.Setup(x => x.GenericRepository<Chat>().GetById(It.IsAny<int>()))
                            .Returns(testChat);
            DeleteChatCommandHandler sut = new DeleteChatCommandHandler(_uowMock.Object);

            _ = await sut.Handle(testCommand, CancellationToken.None);

            _uowMock.Verify(x => x.GenericRepository<Chat>().Delete(testCommand.ChatId), Times.Once);
        }

        [Fact]
        public async Task Handle_ValidRequest_SavesChanges()
        {
            Chat testChat = GenerateChatForTest();
            DeleteChatCommand testCommand = GenerateCommandForTest();
            _uowMock.Setup(x => x.GenericRepository<Chat>().GetById(It.IsAny<int>()))
                            .Returns(testChat);
            DeleteChatCommandHandler sut = new DeleteChatCommandHandler(_uowMock.Object);

            _ = await sut.Handle(testCommand, CancellationToken.None);

            _uowMock.Verify(x => x.SaveChanges(), Times.Once);
        }

        private Chat GenerateChatForTest(int id = 1, string userId = "user1")
        {
            return new Chat() { Id = id, ChatOwnerId = userId };
        }

        private DeleteChatCommand GenerateCommandForTest(int id = 1, string userId = "user1")
        {
            return new DeleteChatCommand() { ChatId = id, UserId = userId };
        }
    }
}
