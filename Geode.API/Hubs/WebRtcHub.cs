﻿using DataAccess.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace Geode.API.Hubs
{
    [Authorize]
    public class WebRtcHub : Hub
    {
        private static readonly List<RtcUser> _users = new List<RtcUser>();

        public async Task JoinCall(string receiverName)
        {
            RtcUser newUser = new RtcUser()
            {
                Username = Context.User!.FindFirstValue(ClaimTypes.Name)!,
                ConnectionId = Context.ConnectionId,
            };
            _users.Add(newUser);

            RtcUser? userInCall = FindUser(receiverName);

            if (userInCall != null)
            {
                await Clients.Client(Context.ConnectionId).SendAsync("InitiateOffer", userInCall.Username);
            }
        }

        public async Task SendOffer(string receiver, string offer)
        {
            RtcUser? existingUser = FindUser(receiver);
            if (existingUser != null)
            {
                await Clients.Client(existingUser.ConnectionId!).SendAsync("ReceiveOffer", offer);
            }
        }

        [Authorize]
        public async Task SendAnswer(string username, string answer)
        {
            RtcUser? existingUser = FindUser(username);
            if (existingUser != null)
            {
                await Clients.Client(existingUser.ConnectionId!).SendAsync("ReceiveAnswer", answer);
            }
        }

        public async Task ProcessCandidate(string receiverName, string candidate)
        {
            RtcUser? userInCall = FindUser(receiverName);

            if (userInCall != null)
            {
                await Clients.Client(userInCall.ConnectionId!).SendAsync("ReceiveCandidate", candidate);
            }
        }

        public async Task CleanUserData()
        {
            RtcUser? currentUser = FindUser(Context.User!.FindFirstValue(ClaimTypes.Name)!);
            if (currentUser != null)
            {
                _users.Remove(currentUser);
            }
        }

        private RtcUser? FindUser(string username)
        {
            return _users.FirstOrDefault(u => u.Username == username);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var existingUser = _users.Find(u => u.ConnectionId == Context.ConnectionId);
            if (existingUser != null)
            {
                _users.Remove(existingUser);
            }

            await base.OnDisconnectedAsync(exception);
        }
    }

    public class RtcUser
    {
        public string? ConnectionId { get; set; }
        public string? Username { get; set; }
    }
}
