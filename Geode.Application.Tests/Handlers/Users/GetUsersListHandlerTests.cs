using Application.Dtos;
using Application.Handlers.Messages;
using Application.Handlers.Users;
using Application.Services.Messages;
using Application.Services.Users;
using Application.Utils.Helpers.Interfaces;
using AutoMapper;
using DataAccess.Entities;
using DataAccess.UnitOfWork;
using Microsoft.Identity.Client;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Geode.Application.Tests.Handlers.Users
{
    public class GetUsersListHandlerTests
    {
        private readonly Mock<IUnitOfWork> _uowMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IRepositoryParametersHelper> _parametersHelper;

        public GetUsersListHandlerTests()
        {
            _uowMock = new Mock<IUnitOfWork>();
            _uowMock.Setup(x => x.GenericRepository<User>().GetList(
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
            GetUsersListQueryHandler sut = new GetUsersListQueryHandler(
                _uowMock.Object,
                _mapperMock.Object,
                _parametersHelper.Object);

            _ = await sut.Handle(new GetUsersListQuery(), CancellationToken.None);

            _parametersHelper.Verify(x => x.GenerateSearchParametersDictionary(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task Handle_Invocation_PreparesSelectProperties()
        {
            Dictionary<string, string> dictionaryFromHelper = new Dictionary<string, string>();
            _parametersHelper.Setup(x => x.GenerateSearchParametersDictionary(It.IsAny<string>()))
                .Returns(dictionaryFromHelper);
            GetUsersListQueryHandler sut = new GetUsersListQueryHandler(
                _uowMock.Object,
                _mapperMock.Object,
                _parametersHelper.Object);

            _ = await sut.Handle(new GetUsersListQuery(), CancellationToken.None);

            _parametersHelper.Verify(x => x.SplitSelectProperties(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task Handle_Invoction_ReturnsResultFromUnitOfWork()
        {
            IQueryable<User> resultFromUow = new List<User>().AsQueryable();
            IEnumerable<UserInfoDto> expected = new List<UserInfoDto>();
            _uowMock.Setup(x => x.GenericRepository<User>().GetList(
                    It.IsAny<Dictionary<string, string>>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<int?>(),
                    It.IsAny<int?>(),
                    It.IsAny<IEnumerable<string>>()))
                    .Returns(resultFromUow);
            _mapperMock.Setup(x => x.Map<IEnumerable<UserInfoDto>>(resultFromUow))
                .Returns(expected);
            Dictionary<string, string> dictionaryFromHelper = new Dictionary<string, string>();
            _parametersHelper.Setup(x => x.GenerateSearchParametersDictionary(It.IsAny<string>()))
                .Returns(dictionaryFromHelper);
            GetUsersListQueryHandler sut = new GetUsersListQueryHandler(
                _uowMock.Object,
                _mapperMock.Object,
                _parametersHelper.Object);

            IEnumerable<UserInfoDto> actual = await sut.Handle(new GetUsersListQuery(), CancellationToken.None);

            Assert.Same(expected, actual);
        }
    }
}
