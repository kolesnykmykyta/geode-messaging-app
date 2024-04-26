using Application.Utils.Helpers;
using AutoMapper;
using DataAccess.Entities;
using DataAccess.Repositories;
using DataAccess.Repositories.Interfaces;
using DataAccess.UnitOfWork;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Geode.Application.Tests.Utils.Helpers
{
    public class ChatRepositoryHelperTests
    {
        private const string userId = "test";
        private const int chatId = 1;

        private readonly Mock<IGenericRepository<Chat>> _chatRepoMock;
        private readonly Mock<IGenericRepository<ChatMember>> _chatMemberRepoMock;
        private readonly Mock<IUnitOfWork> _uowMock;
        private readonly Mock<IMapper> _mapperMock;

        public ChatRepositoryHelperTests()
        {
            _chatRepoMock = new Mock<IGenericRepository<Chat>>();
            _chatMemberRepoMock = new Mock<IGenericRepository<ChatMember>>();

            _uowMock = new Mock<IUnitOfWork>();
            _uowMock.Setup(x => x.GenericRepository<Chat>())
                .Returns(_chatRepoMock.Object);
            _uowMock.Setup(x => x.GenericRepository<ChatMember>())
                .Returns(_chatMemberRepoMock.Object);

            _mapperMock = new Mock<IMapper>();
        }

        [Fact]
        public void CreateNewChat_InsertsChatAndMember()
        {
            ChatRepositoryHelper sut = new ChatRepositoryHelper(_uowMock.Object, _mapperMock.Object);

            sut.CreateNewChat(new Chat());

            _chatRepoMock.Verify(x => x.Insert(It.IsAny<Chat>()), Times.Once);
            _chatMemberRepoMock.Verify(x => x.Insert(It.IsAny<ChatMember>()), Times.Once);
        }

        [Fact]
        public void CreateNewChat_SavesChanges()
        {
            ChatRepositoryHelper sut = new ChatRepositoryHelper(_uowMock.Object, _mapperMock.Object);

            sut.CreateNewChat(new Chat());

            _uowMock.Verify(x => x.SaveChanges(), Times.Once);
        }

        [Fact]
        public void JoinChat_NotExistingChat_ReturnsFalse()
        {
            ChatRepositoryHelper sut = new ChatRepositoryHelper(_uowMock.Object, _mapperMock.Object);

            bool actual = sut.JoinChat(chatId, userId);

            Assert.False(actual);
        }

        [Fact]
        public void JoinChat_NotExistingChat_DoesNothing()
        {
            ChatRepositoryHelper sut = new ChatRepositoryHelper(_uowMock.Object, _mapperMock.Object);

            _ = sut.JoinChat(chatId, userId);

            _chatMemberRepoMock.Verify(x => x.Insert(It.IsAny<ChatMember>()), Times.Never);
        }

        [Fact]
        public void JoinChat_UserAlreadyInChat_ReturnsFalse()
        {
            SetupChatMemberRepositoryToReturnMember();
            SetupChatRepositoryToReturnChat();
            ChatRepositoryHelper sut = new ChatRepositoryHelper(_uowMock.Object, _mapperMock.Object);

            bool actual = sut.JoinChat(chatId, userId);

            Assert.False(actual);
        }

        [Fact]
        public void JoinChat_UserAlreadyInChat_DoesNothing()
        {
            SetupChatMemberRepositoryToReturnMember();
            SetupChatRepositoryToReturnChat();
            ChatRepositoryHelper sut = new ChatRepositoryHelper(_uowMock.Object, _mapperMock.Object);

            _ = sut.JoinChat(chatId, userId);

            _chatMemberRepoMock.Verify(x => x.Insert(It.IsAny<ChatMember>()), Times.Never);
        }

        [Fact]
        public void JoinChat_ValidRequest_ReturnsTrue()
        {
            SetupChatRepositoryToReturnChat();
            ChatRepositoryHelper sut = new ChatRepositoryHelper(_uowMock.Object, _mapperMock.Object);

            bool actual = sut.JoinChat(chatId, userId);

            Assert.True(actual);
        }

        [Fact]
        public void JoinChat_ValidRequest_AddsChatMemberToDatabase()
        {
            SetupChatRepositoryToReturnChat();
            ChatRepositoryHelper sut = new ChatRepositoryHelper(_uowMock.Object, _mapperMock.Object);

            _ = sut.JoinChat(chatId, userId);

            _chatMemberRepoMock.Verify(x => x.Insert(It.IsAny<ChatMember>()), Times.Once);
        }

        [Fact]
        public void JoinChat_ValidRequest_SavesChanges()
        {
            SetupChatRepositoryToReturnChat();
            ChatRepositoryHelper sut = new ChatRepositoryHelper(_uowMock.Object, _mapperMock.Object);

            _ = sut.JoinChat(chatId, userId);

            _uowMock.Verify(x => x.SaveChanges(), Times.Once);
        }

        [Fact]
        public void LeaveChat_NotExistingMember_ReturnsFalse()
        {
            SetupChatRepositoryToReturnChat();
            ChatRepositoryHelper sut = new ChatRepositoryHelper(_uowMock.Object, _mapperMock.Object);

            bool actual = sut.LeaveChat(chatId, userId);

            Assert.False(actual);
        }

        [Fact]
        public void LeaveChat_NotExistingMember_DoesNothing()
        {
            SetupChatRepositoryToReturnChat();
            ChatRepositoryHelper sut = new ChatRepositoryHelper(_uowMock.Object, _mapperMock.Object);

            _ = sut.LeaveChat(chatId, userId);

            _chatMemberRepoMock.Verify(x => x.Delete(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public void LeaveChat_UserIsChatOwner_ReturnsFalse()
        {
            SetupChatMemberRepositoryToReturnMember();
            SetupChatRepositoryToReturnChat(true);
            ChatRepositoryHelper sut = new ChatRepositoryHelper(_uowMock.Object, _mapperMock.Object);

            bool actual = sut.LeaveChat(chatId, userId);

            Assert.False(actual);
        }

        [Fact]
        public void LeaveChat_UserIsChatOwner_DoesNothing()
        {
            SetupChatMemberRepositoryToReturnMember();
            SetupChatRepositoryToReturnChat(true);
            ChatRepositoryHelper sut = new ChatRepositoryHelper(_uowMock.Object, _mapperMock.Object);

            _ = sut.LeaveChat(chatId, userId);

            _chatMemberRepoMock.Verify(x => x.Delete(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public void LeaveChat_ValidRequest_ReturnsTrue()
        {
            SetupChatMemberRepositoryToReturnMember();
            SetupChatRepositoryToReturnChat();
            ChatRepositoryHelper sut = new ChatRepositoryHelper(_uowMock.Object, _mapperMock.Object);

            bool actual = sut.LeaveChat(chatId, userId);

            Assert.True(actual);
        }

        [Fact]
        public void LeaveChat_ValidRequest_UpdatesDatabase()
        {
            SetupChatMemberRepositoryToReturnMember();
            SetupChatRepositoryToReturnChat();
            ChatRepositoryHelper sut = new ChatRepositoryHelper(_uowMock.Object, _mapperMock.Object);

            _ = sut.LeaveChat(chatId, userId);

            _chatMemberRepoMock.Verify(x => x.Delete(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void LeaveChat_ValidRequest_SavesChanges()
        {
            SetupChatMemberRepositoryToReturnMember();
            SetupChatRepositoryToReturnChat();
            ChatRepositoryHelper sut = new ChatRepositoryHelper(_uowMock.Object, _mapperMock.Object);

            _ = sut.LeaveChat(chatId, userId);

            _uowMock.Verify(x => x.SaveChanges(), Times.Once);
        }

        private void SetupChatMemberRepositoryToReturnMember()
        {
            List<ChatMember> returnFromMemberRepo = new List<ChatMember>()
            {
                new ChatMember(){ ChatId = chatId, UserId = userId}
            };

            _chatMemberRepoMock.Setup(x => x.GetList(
                It.IsAny<Dictionary<string, string>>(),
                It.IsAny<string>(),
                It.IsAny<bool>(),
                It.IsAny<int?>(),
                It.IsAny<int?>(),
                It.IsAny<IEnumerable<string>>()))
                .Returns(returnFromMemberRepo.AsQueryable());
        }

        private void SetupChatRepositoryToReturnChat(bool isUserOwner = false)
        {
            _chatRepoMock.Setup(x => x.GetById(chatId))
                .Returns(new Chat() { ChatOwnerId = isUserOwner ? userId : "notowner"});
        }
    }
}
