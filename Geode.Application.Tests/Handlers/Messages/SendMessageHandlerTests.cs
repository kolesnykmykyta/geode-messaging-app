using Application.Dtos;
using Application.Handlers.Messages;
using Application.Services.Messages;
using AutoMapper;
using DataAccess.Entities;
using DataAccess.Repositories.Interfaces;
using DataAccess.UnitOfWork;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Geode.Application.Tests.Handlers.Messages
{
    public class SendMessageHandlerTests
    {
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IUnitOfWork> _uowMock;

        public SendMessageHandlerTests()
        {
            _mapperMock = new Mock<IMapper>();
            
            _uowMock = new Mock<IUnitOfWork>();
            _uowMock.Setup(x => x.GenericRepository<Message>())
                .Returns(new Mock<IGenericRepository<Message>>().Object);
        }

        [Fact]
        public async Task Handle_Invocation_SavesChanges()
        {
            Message testMessage = new Message();
            _mapperMock.Setup(x => x.Map<Message>(It.IsAny<SendMessageCommand>()))
                .Returns(testMessage);
            SendMessageCommandHandler sut = new SendMessageCommandHandler(_mapperMock.Object, _uowMock.Object);

            await sut.Handle(new SendMessageCommand(), CancellationToken.None);

            _uowMock.Verify(x => x.SaveChanges(), Times.Once);
        }

        [Fact]
        public async Task Handle_Invocation_SetsSentAtValue()
        {
            Message testMessage = new Message();
            _mapperMock.Setup(x => x.Map<Message>(It.IsAny<SendMessageCommand>()))
                .Returns(testMessage);
            SendMessageCommandHandler sut = new SendMessageCommandHandler(_mapperMock.Object, _uowMock.Object);

            await sut.Handle(new SendMessageCommand(), CancellationToken.None);

            Assert.NotNull(testMessage.SentAt);
        }

        [Fact]
        public async Task Handle_Invocation_UpdatesDatabase()
        {
            Message testMessage = new Message();
            _mapperMock.Setup(x => x.Map<Message>(It.IsAny<SendMessageCommand>()))
                .Returns(testMessage);
            SendMessageCommandHandler sut = new SendMessageCommandHandler(_mapperMock.Object, _uowMock.Object);

            await sut.Handle(new SendMessageCommand(), CancellationToken.None);

            _uowMock.Verify(x => x.GenericRepository<Message>().Insert(It.IsAny<Message>()), Times.Once);
        }
    }
}
