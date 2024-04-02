using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Geode.Maui.Auth
{
    internal class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private ClaimsPrincipal currentUser = new ClaimsPrincipal(new ClaimsIdentity());

        public override Task<AuthenticationState> GetAuthenticationStateAsync() =>
            Task.FromResult(new AuthenticationState(currentUser));

        public Task LogInAsync(string email, string password)
        {
            var loginTask = LogInAsyncCore();
            NotifyAuthenticationStateChanged(loginTask);

            return loginTask;

            async Task<AuthenticationState> LogInAsyncCore()
            {
                var user = await LoginWithExternalProviderAsync();
                currentUser = user;

                return new AuthenticationState(currentUser);
            }
        }

        private Task<ClaimsPrincipal> LoginWithExternalProviderAsync()
        {
            ClaimsIdentity identity = new ClaimsIdentity("Custom");

            var authenticatedUser = new ClaimsPrincipal(identity);

            return Task.FromResult(authenticatedUser);
        }

        public void Logout()
        {
            currentUser = new ClaimsPrincipal(new ClaimsIdentity());
            NotifyAuthenticationStateChanged(
                Task.FromResult(new AuthenticationState(currentUser)));
        }
    }
}
