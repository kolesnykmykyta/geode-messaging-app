using Application.Dtos;
using Blazored.LocalStorage;
using Geode.Maui.Services.Interfaces;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geode.Maui.Services
{
    internal class ChatHubService : IChatHubService
    {
        public event Action<ChatMessageDto> OnMessageReceived;
        private HubConnection _hubConnection;
        private readonly ILocalStorageService _localStorage;

        public ChatHubService(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public async Task ConnectToChatHubAsync(int chatId)
        {
            string token = await _localStorage.GetItemAsStringAsync("BearerToken");

            _hubConnection = new HubConnectionBuilder()
            .WithUrl("https://localhost:7077/chathub", options =>
            {
                options.AccessTokenProvider = () => Task.FromResult(token);
            })
            .Build();

            _hubConnection.On<string, string>("ReceiveMessage", (message, sender) =>
            {
                ChatMessageDto newMessage = new ChatMessageDto()
                {
                    Content = message,
                    Sender = sender,
                };

                OnMessageReceived?.Invoke(newMessage);
            });

            await _hubConnection.StartAsync();

            await _hubConnection.InvokeAsync("JoinChat", chatId);
        }

        public async Task SendMessageAsync(int chatId, string message)
        {
            await _hubConnection.SendAsync("SendMessage", chatId, message);
        }
    }
}
