using Application.Handlers.Chats;
using Application.Services.Chats;
using AutoMapper;
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
    public class ChangeChatNameHandlerTests
    {
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IUnitOfWork> _uowMock;

        public ChangeChatNameHandlerTests()
        {
            _mapperMock = new Mock<IMapper>();
            _mapperMock.Setup(x => x.Map<Chat>(It.IsAny<ChangeChatNameCommand>()))
                .Returns(new Chat());

            _uowMock = new Mock<IUnitOfWork>();
        }

        [Fact]
        public async Task Handle_NonExistingChat_ReturnsFalse()
        {
            _uowMock.Setup(x => x.GenericRepository<Chat>().GetById(It.IsAny<int>()))
                .Returns((Chat?)null);
            ChangeChatNameCommandHandler sut = new ChangeChatNameCommandHandler(_mapperMock.Object, _uowMock.Object);

            bool actual = await sut.Handle(new ChangeChatNameCommand(), CancellationToken.None);

            Assert.False(actual);
        }

        [Fact]
        public async Task Handle_UserIsNotOwner_ReturnsFalse()
        {
            Chat testChat = GenerateChatForTest();
            ChangeChatNameCommand testCommand = GenerateCommandForTest(userId: "user2");
            _uowMock.Setup(x => x.GenericRepository<Chat>().GetById(1))
                .Returns(testChat);
            ChangeChatNameCommandHandler sut = new ChangeChatNameCommandHandler(_mapperMock.Object, _uowMock.Object);

            bool actual = await sut.Handle(testCommand, CancellationToken.None);
                
            Assert.False(actual);
        }

        [Fact]
        public async Task Handle_ValidRequest_ReturnsTrue()
        {
            Chat chatFromMock = GenerateChatForTest();
            ChangeChatNameCommand testCommand = GenerateCommandForTest();
            _uowMock.Setup(x => x.GenericRepository<Chat>().GetById(1))
                .Returns(chatFromMock);
            ChangeChatNameCommandHandler sut = new ChangeChatNameCommandHandler(_mapperMock.Object, _uowMock.Object);

            bool actual = await sut.Handle(testCommand, CancellationToken.None);

            Assert.True(actual);
        }

        [Fact]
        public async Task Handle_ValidRequest_UpdatesDatabase()
        {
            Chat chatFromMock = GenerateChatForTest();
            ChangeChatNameCommand testCommand = GenerateCommandForTest();
            _uowMock.Setup(x => x.GenericRepository<Chat>().GetById(1))
                .Returns(chatFromMock);
            _mapperMock.Setup(x => x.Map<Chat>(testCommand))
                .Returns(chatFromMock);
            ChangeChatNameCommandHandler sut = new ChangeChatNameCommandHandler(_mapperMock.Object, _uowMock.Object);

            _ = await sut.Handle(testCommand, CancellationToken.None);

            _uowMock.Verify(x => x.GenericRepository<Chat>().Update(testCommand.Id, It.Is<Chat>(x => x.Id == testCommand.Id)), Times.Once);
        }

        [Fact]
        public async Task Handle_ValidRequst_SavesChanges()
        {
            Chat chatFromMock = GenerateChatForTest();
            ChangeChatNameCommand testCommand = GenerateCommandForTest();
            _uowMock.Setup(x => x.GenericRepository<Chat>().GetById(1))
                .Returns(chatFromMock);
            ChangeChatNameCommandHandler sut = new ChangeChatNameCommandHandler(_mapperMock.Object, _uowMock.Object);

            _ = await sut.Handle(testCommand, CancellationToken.None);

            _uowMock.Verify(x => x.SaveChanges(), Times.Once);
        }

        private Chat GenerateChatForTest(int id = 1, string userId = "user1")
        {
            return new Chat() { Id = id, ChatOwnerId = userId };
        }

        private ChangeChatNameCommand GenerateCommandForTest(int id = 1, string userId = "user1")
        {
            return new ChangeChatNameCommand() { Id = id, ChatOwnerId = userId };
        }
    }
}
