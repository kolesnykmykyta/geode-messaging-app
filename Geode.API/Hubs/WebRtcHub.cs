using DataAccess.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace Geode.API.Hubs
{
    [Authorize]
    public class WebRtcHub : Hub
    {
        private static readonly List<RtcUser> _users = new List<RtcUser>();

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

        public async Task SendAnswer(string username, string answer)
        {
            RtcUser? existingUser = FindUser(username);
            if (existingUser != null)
            {
                await Clients.Client(existingUser.ConnectionId!).SendAsync("ReceiveAnswer", answer);
            }
        }

        public async Task SendCandidate(string username, string candidate)
        {
            RtcUser? existingUser = FindUser(username);
            if (existingUser != null)
            {
                await Clients.Client(existingUser.ConnectionId).SendAsync("ReceiveCandidate", candidate);
            }
        }

        public async Task JoinCall(string username)
        {
            RtcUser? existingUser = FindUser(username);
            if (existingUser != null)
            {
                await Clients.Client(Context.ConnectionId).SendAsync("ReceiveOffer", existingUser.Offer);

                if (existingUser.Candidates != null)
                {
                    foreach (string candidate in existingUser.Candidates)
                    {
                        await Clients.Client(Context.ConnectionId).SendAsync("ReceiveCandidate", candidate);
                    }
                }
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
        public string? Offer { get; set; }
        public List<string>? Candidates { get; set; }
    }
}
