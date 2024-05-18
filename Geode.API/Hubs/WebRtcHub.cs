using Application.Utils.Helpers.Interfaces;
using DataAccess.Entities;
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
        private readonly IApiUserHelper _userHelper;

        public WebRtcHub(IApiUserHelper userHelper)
        {
            _userHelper = userHelper;
        }

        public async Task JoinPrivateCall(string receiverName)
        {
            string currentUserName = _userHelper.ExtractNameFromUser(Context.User!);
            RtcUser newUser = new RtcUser()
            {
                Username = currentUserName,
                ConnectionId = Context.ConnectionId,    
                GroupName = receiverName,
            };
            _users.Add(newUser);

            RtcUser? receiver = _users.FirstOrDefault(x => x.Username == receiverName && x.GroupName == currentUserName);

            if (receiver != null)
            {
                await Clients.Client(Context.ConnectionId).SendAsync("InitiateOffer", receiver.Username);
            }
        }

        public async Task JoinCall(string groupName)
        {
            IEnumerable<RtcUser> usersInCall = _users.Where(x => x.GroupName == groupName);
            if (usersInCall.Any())
            {
                foreach(RtcUser user in usersInCall)
                {
                    await Clients.Client(Context.ConnectionId).SendAsync("InitiateOffer", user.Username);
                }
            }

            RtcUser newUser = new RtcUser()
            {
                Username = _userHelper.ExtractNameFromUser(Context.User!),
                ConnectionId = Context.ConnectionId,
                GroupName = groupName,
            };
            _users.Add(newUser);
        }

        public async Task SendOffer(string receiverName, string offer)
        {
            RtcUser? receiver = FindUser(receiverName);
            if (receiver != null)
            {
                string senderName = _userHelper.ExtractNameFromUser(Context.User!);
                await Clients.Client(receiver.ConnectionId!).SendAsync("ReceiveOffer", senderName, offer);
            }
        }

        public async Task SendAnswer(string receiverName, string answer)
        {
            RtcUser? receiver = FindUser(receiverName);
            if (receiver != null)
            {
                string senderName = _userHelper.ExtractNameFromUser(Context.User!);
                await Clients.Client(receiver.ConnectionId!).SendAsync("ReceiveAnswer", senderName, answer);
            }
        }

        public async Task ProcessCandidate(string receiverName, string candidate)
        {
            RtcUser? receiver = FindUser(receiverName);

            if (receiver != null)
            {
                string senderName = _userHelper.ExtractNameFromUser(Context.User!);
                await Clients.Client(receiver.ConnectionId!).SendAsync("ReceiveCandidate", senderName, candidate);
            }
        }

        public void CleanUserData()
        {
            RtcUser? currentUser = FindUser(_userHelper.ExtractNameFromUser(Context.User!));
            if (currentUser != null)
            {
                _users.Remove(currentUser);
            }
        }

        public async Task RemoveUserVideo(string receiverName)
        {
            RtcUser? receiver = FindUser(receiverName);
            if (receiver != null)
            {
                string senderName = _userHelper.ExtractNameFromUser(Context.User!);
                await Clients.Client(receiver.ConnectionId!).SendAsync("RemovePeerVideo", senderName);
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

        public string? GroupName { get; set; }
    }
}
