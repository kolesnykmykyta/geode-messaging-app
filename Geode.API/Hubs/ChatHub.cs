using Application.Services.Messages;
using Application.Utils.Helpers.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace Geode.API.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly IApiUserHelper _userHelper;
        private readonly IMediator _mediator;

        public ChatHub(IApiUserHelper userHelper, IMediator mediator)
        {
            _userHelper = userHelper;
            _mediator = mediator;
        }

        public async Task JoinChat(int chatId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"chat-{chatId}");
        }

        public async Task SendMessage(int chatId, string message)
        {
            string userId = _userHelper.ExtractIdFromUser(Context.GetHttpContext()!.User);
            string sender = Context.GetHttpContext()!.User.FindFirst(ClaimTypes.Name)!.Value;

            SendMessageCommand command = new SendMessageCommand()
            {
                ChatId = chatId,
                SenderId = userId,
                Content = message
            };
            await _mediator.Send(command);

            await Clients.Group($"chat-{chatId}").SendAsync("ReceiveMessage", message, sender);
        }
    }
}
