using Application.Dtos;
using Application.Handlers.Chats;
using Application.Handlers.Messages;
using Application.Services.Messages;
using Application.Utils.Helpers.Interfaces;
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

namespace Geode.Application.Tests.Handlers.Messages
{
    public class GetUserMessagesHandlerTests
    {
        private readonly Mock<IUnitOfWork> _uowMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IRepositoryParametersHelper> _parametersHelper;

        public GetUserMessagesHandlerTests()
        {
            _uowMock = new Mock<IUnitOfWork>();
            _uowMock.Setup(x => x.GenericRepository<Message>().GetList(
                It.IsAny<Dictionary<string, string>>(),
                It.IsAny<string>(),
                It.IsAny<bool>(),
                It.IsAny<int?>(),
                It.IsAny<int?>(),
                It.IsAny<IEnumerable<string>>()))
                .Returns(() => null);
            _mapperMock = new Mock<IMapper>();
            _parametersHelper = new Mock<IRepositoryParametersHelper>();
        }

        [Fact]
        public async Task Handle_Invocation_PreparesSearchParameters()
        {
            Dictionary<string, string> dictionaryFromHelper = new Dictionary<string, string>();
            _parametersHelper.Setup(x => x.GenerateSearchParametersDictionary(It.IsAny<string>()))
                .Returns(dictionaryFromHelper);
            GetUserMessagesQueryHandler sut = new GetUserMessagesQueryHandler(
                _uowMock.Object,
                _mapperMock.Object,
                _parametersHelper.Object);

            _ = await sut.Handle(new GetUserMessagesQuery() { SenderId = "Test"}, CancellationToken.None);

            _parametersHelper.Verify(x => x.GenerateSearchParametersDictionary(It.IsAny<string>()), Times.Once);
            Assert.True(dictionaryFromHelper.ContainsKey("SenderId"));
            Assert.True(dictionaryFromHelper["SenderId"] == "Test");
        }

        [Fact]
        public async Task Handle_Invocation_PreparesSelectProperties()
        {
            Dictionary<string, string> dictionaryFromHelper = new Dictionary<string, string>();
            _parametersHelper.Setup(x => x.GenerateSearchParametersDictionary(It.IsAny<string>()))
                .Returns(dictionaryFromHelper);
            GetUserMessagesQueryHandler sut = new GetUserMessagesQueryHandler(
                _uowMock.Object,
                _mapperMock.Object,
                _parametersHelper.Object);

            _ = await sut.Handle(new GetUserMessagesQuery() { SenderId = "Test" }, CancellationToken.None);

            _parametersHelper.Verify(x => x.SplitSelectProperties(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task Handle_Invocation_ReturnsResultFromUnitOfWork()
        {
            List<MessageDto> expected = new List<MessageDto>();
            _mapperMock.Setup(x => x.Map<IEnumerable<MessageDto>>(It.IsAny<IQueryable<Message>>()))
                .Returns(expected);
            Dictionary<string, string> dictionaryFromHelper = new Dictionary<string, string>();
            _parametersHelper.Setup(x => x.GenerateSearchParametersDictionary(It.IsAny<string>()))
                .Returns(dictionaryFromHelper);
            GetUserMessagesQueryHandler sut = new GetUserMessagesQueryHandler(
                _uowMock.Object,
                _mapperMock.Object,
                _parametersHelper.Object);

            IEnumerable<MessageDto> actual = await sut.Handle(new GetUserMessagesQuery(), CancellationToken.None);

            Assert.Equal(expected, actual);
        }
    }
}
