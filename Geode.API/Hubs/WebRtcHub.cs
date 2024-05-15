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

        public async Task JoinCall(string receiverName, string offer)
        {
            RtcUser? userInCall = FindUser(receiverName);

            if (userInCall == null)
            {
                string username = Context.User!.FindFirstValue(ClaimTypes.Name)!;
                RtcUser newUser = new RtcUser()
                {
                    Username = username,
                    ConnectionId = Context.ConnectionId,
                    Offer = offer,
                };

                _users.Add(newUser);
            }
            else
            {
                await Clients.Client(Context.ConnectionId).SendAsync("ReceiveOffer", userInCall.Offer);

                if (userInCall.Candidates != null)
                {
                    foreach (string candidate in userInCall.Candidates)
                    {
                        await Clients.Client(Context.ConnectionId).SendAsync("ReceiveCandidate", candidate);
                    }
                }
            }
        }

        public async Task ProcessCandidate(string receiverName, string candidate)
        {
            RtcUser? userInCall = FindUser(receiverName);

            if (userInCall == null)
            {
                EnsureUserIsInList();
                RtcUser currentUser = FindUser(Context.User!.FindFirstValue(ClaimTypes.Name)!)!;

                if (currentUser.Candidates == null)
                {
                    currentUser.Candidates = new List<string>();
                }

                currentUser.Candidates.Add(candidate);
            }
            else
            {
                await Clients.Client(userInCall.ConnectionId!).SendAsync("ReceiveCandidate", candidate);
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

        // LEGACY
        [Authorize]
        public async Task StoreOffer(string offer)
        {
            string username = Context.User!.FindFirstValue(ClaimTypes.Name)!;
            RtcUser? existingUser = FindUser(username);
            if (existingUser == null)
            {
                RtcUser newUser = new RtcUser() { Username = username, ConnectionId = Context.ConnectionId };
                _users.Add(newUser);
                existingUser = newUser;
            }

            existingUser.Offer = offer;
        }

        [Authorize]
        public async Task StoreCandidate(string candidate)
        {
            string username = Context.User!.FindFirstValue(ClaimTypes.Name)!;
            RtcUser? existingUser = FindUser(username);
            if (existingUser != null)
            {
                if (existingUser.Candidates == null)
                {
                    existingUser.Candidates = new List<string>();
                }

                existingUser.Candidates.Add(candidate);
            }
        }

        [Authorize]
        public async Task SendCandidate(string username, string candidate)
        {
            RtcUser? existingUser = FindUser(username);
            if (existingUser != null)
            {
                await Clients.Client(existingUser.ConnectionId).SendAsync("ReceiveCandidate", candidate);
            }
        }

        private RtcUser? FindUser(string username)
        {
            return _users.FirstOrDefault(u => u.Username == username);
        }

        private void EnsureUserIsInList()
        {
            string currentUserName = Context.User!.FindFirstValue(ClaimTypes.Name)!;

            if (FindUser(currentUserName) == null)
            {
                RtcUser newUser = new RtcUser()
                {
                    Username = currentUserName,
                    ConnectionId = Context.ConnectionId,
                    Candidates = new List<string>(),
                };
            }
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
        public string? Offer { get; set; }
        public List<string>? Candidates { get; set; }
    }
}
