using Application.Dtos;
using Application.Handlers.Chats;
using Application.Services.Chats;
using Application.Utils.Helpers.Interfaces;
using AutoMapper;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Geode.Application.Tests.Handlers.Chats
{
    public class GetUserChatsHandlerTests
    {
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IChatRepositoryHelper> _repositoryHelperMock;
        private readonly Mock<IRepositoryParametersHelper> _parametersHelper;

        public GetUserChatsHandlerTests()
        {
            _mapperMock = new Mock<IMapper>();
            _repositoryHelperMock = new Mock<IChatRepositoryHelper>();
            _parametersHelper = new Mock<IRepositoryParametersHelper>();
        }

        [Fact]
        public async Task Handle_Invocation_PreparesSearchParameters()
        {
            GetUserChatsQueryHandler sut = new GetUserChatsQueryHandler(
                _repositoryHelperMock.Object,
                _mapperMock.Object,
                _parametersHelper.Object);

            _ = await sut.Handle(new GetUserChatsQuery(), CancellationToken.None);

            _parametersHelper.Verify(x => x.GenerateSearchParametersDictionary(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task Handle_Invocation_PreparesSelectParams()
        {
            GetUserChatsQueryHandler sut = new GetUserChatsQueryHandler(
                _repositoryHelperMock.Object,
                _mapperMock.Object,
                _parametersHelper.Object);

            _ = await sut.Handle(new GetUserChatsQuery(), CancellationToken.None);

            _parametersHelper.Verify(x => x.SplitSelectProperties(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task Handle_Invocation_ReturnsResultFromHelper()
        {
            List<ChatDto> expected = new List<ChatDto>();
            _repositoryHelperMock.Setup(x => x.GetUserChats(
                It.IsAny<string>(),
                It.IsAny<Dictionary<string, string>>(),
                It.IsAny<string>(),
                It.IsAny<bool>(),
                It.IsAny<int?>(),
                It.IsAny<int?>(),
                It.IsAny<IEnumerable<string>>()))
                .Returns(expected);
            GetUserChatsQueryHandler sut = new GetUserChatsQueryHandler(
                _repositoryHelperMock.Object,
                _mapperMock.Object,
                _parametersHelper.Object);

            IEnumerable<ChatDto> actual = await sut.Handle(new GetUserChatsQuery(), CancellationToken.None);

            Assert.Same(expected, actual);
        }
    }
}
