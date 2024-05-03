using Application.Services.Messages;
using Application.Utils.Helpers.Interfaces;
using Geode.API.Hubs;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Geode.Api.Tests.Hubs
{
    public class ChatHubTests
    {
        private const string ConnectionId = "connId";

        private readonly Mock<IGroupManager> _groupManagerMock;
        private readonly Mock<IHubCallerClients> _clientsMock;
        private readonly Mock<IClientProxy> _clientProxyMock;
        private readonly Mock<HubCallerContext> _contextMock;
        private readonly Mock<IApiUserHelper> _apiUserHelperMock;
        private readonly Mock<IMediator> _meditorMock;

        public ChatHubTests()
        {
            _groupManagerMock = new Mock<IGroupManager>();

            _clientsMock = new Mock<IHubCallerClients>();

            _clientProxyMock = new Mock<IClientProxy>();

            _contextMock = new Mock<HubCallerContext>();
            _contextMock.SetupGet(x => x.ConnectionId)
                .Returns(ConnectionId);
            _contextMock.SetupGet(x => x.User)
                .Returns((ClaimsPrincipal)null);

            _apiUserHelperMock = new Mock<IApiUserHelper>();
            _apiUserHelperMock.Setup(x => x.ExtractIdFromUser(It.IsAny<ClaimsPrincipal>()))
                .Returns("testId");
            _apiUserHelperMock.Setup(x => x.ExtractNameFromUser(It.IsAny<ClaimsPrincipal>()))
                .Returns("testName");

            _meditorMock = new Mock<IMediator>();
        }

        [Fact]
        public async Task JoinChat_AddsUserToExpectedGroup()
        {
            ChatHub sut = new ChatHub(_apiUserHelperMock.Object, _meditorMock.Object)
            {
                Groups = _groupManagerMock.Object,
                Context = _contextMock.Object,
            };

            await sut.JoinChat(1);

            _groupManagerMock.Verify(x => x.AddToGroupAsync(ConnectionId, "chat-1", It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task SendMessage_PassesExcutionToMeditor()
        {
            ChatHub sut = new ChatHub(_apiUserHelperMock.Object, _meditorMock.Object)
            {
                Groups = _groupManagerMock.Object,
                Context = _contextMock.Object,
                Clients = _clientsMock.Object,
            };

            _clientsMock.Setup(x => x.Group("chat-1"))
                .Returns(_clientProxyMock.Object);

            await sut.SendMessage(1, "test");

            _meditorMock.Verify(x => x.Send(It.IsAny<SendMessageCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task SendMessage_NotifiesClients()
        {
            string message = "test";
            ChatHub sut = new ChatHub(_apiUserHelperMock.Object, _meditorMock.Object)
            {
                Groups = _groupManagerMock.Object,
                Context = _contextMock.Object,
                Clients = _clientsMock.Object,
            };

            _clientsMock.Setup(x => x.Group("chat-1"))
                .Returns(_clientProxyMock.Object);

            await sut.SendMessage(1, message);

            _clientProxyMock.Verify
                (x => x.SendCoreAsync(It.IsAny<string>(), It.Is<object[]>(x => x.Contains(message)), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
