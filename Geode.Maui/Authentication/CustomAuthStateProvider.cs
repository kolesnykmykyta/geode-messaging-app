using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Web;
using Application.Utils.HttpClientWrapper;
using Auth.Dtos;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage.Json;

namespace Geode.Maui.Authentication
{
    internal class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private ClaimsPrincipal currentUser = new ClaimsPrincipal(new ClaimsIdentity());

        public override Task<AuthenticationState> GetAuthenticationStateAsync() =>
            Task.FromResult(new AuthenticationState(currentUser));

        public Task LogInAsync(ClaimsPrincipal userPrincipal)
        {
            var loginTask = LogInAsyncCore();
            NotifyAuthenticationStateChanged(loginTask);

            return loginTask;

            Task<AuthenticationState> LogInAsyncCore()
            {
                currentUser = userPrincipal;

                return Task.FromResult(new AuthenticationState(currentUser));
            }
        }

        public void Logout()
        {
            currentUser = new ClaimsPrincipal(new ClaimsIdentity());

            NotifyAuthenticationStateChanged(
                Task.FromResult(new AuthenticationState(currentUser)));
        }
    }
}
